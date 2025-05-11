using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.DBEntity.Transactions;
using Auth.Domain.Core.Data.Queues;
using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;
using Auth.Domain.Interface.Data.Read.Queues;
using Auth.Domain.Interface.Data.Read.UOW;
using Auth.Domain.Interface.Logic.External.Payments;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.TransactionHandlers
{
    internal class CheckPaymentTransactionHandler(IUnitOfWorkRead unitOfWork,
        IPaymentUnitOfWork payment,
        IQueueRepository<WaitingPaymentApproval> queue,
        ICommandHandler<ConfirmPaymentActionCommand> handler,
        IOptionsSnapshot<PaymentOptions> options)
        : Handler<CheckPaymentTransactionCommand, CheckPaymentTransactionResponse>
    {
        private readonly PaymentOptions _options = options.Value;
        private readonly IUnitOfWorkRead _uow = unitOfWork;
        private readonly IPaymentUnitOfWork _payment = payment;
        private readonly ICommandHandler<ConfirmPaymentActionCommand> _handler = handler;
        private readonly IQueueRepository<WaitingPaymentApproval> _queue = queue;
        private TimeSpan BaseTime => TimeSpan.FromMinutes(_options.FrequencyOfTransactionApprovalChecksInMinutes);
        public override async Task<CheckPaymentTransactionResponse> HandleAsync(CheckPaymentTransactionCommand command)
        {
            TransactionPayment transaction = null;
            CheckPaymentTransactionResponse response = new(BaseTime, true);
            if (command.TransactionId.HasValue)
                transaction = await GetTransaction(command.TransactionId.Value);
            if (transaction is null)
                transaction = await GetFromQueueTransaction(response);
            if (!response.IsSuccess)
                return response;
            if (IsDBStatusPending(transaction))
            {
                await PaymentSystemCheckAsync(transaction, response.IsFromQueue);
            }
            if (response.IsFromQueue)
                _queue.TryRemove(out _);

            return response;
        }
        private bool TryAgain(ActionStatus status, bool isFromQueue)
        {
            if (!isFromQueue) return false;
            if (status != ActionStatus.Pending) return false;
            _queue.TryGet(out WaitingPaymentApproval oldestPayment);
            if (oldestPayment.Attempt > 3) return false;

            _queue.Add(oldestPayment with
            {
                Expires = DateTime.UtcNow
                        .AddMinutes(_options.ExpirePendingInMinutes),
                Attempt = (byte)(oldestPayment.Attempt + 1)

            });
            return true;
        }
        private async Task PaymentSystemCheckAsync(
            TransactionPayment transaction, bool isFromQueue)
        {
            var status = await _payment.GetTransactionStatusAsync(transaction.PaymentId, transaction.Type);

            if (TryAgain(status, isFromQueue))
                return;
            await _handler.HandleAsync(new ConfirmPaymentActionCommand
            {
                PaymentId = transaction.PaymentId,
                Status = status,
                TransactionId = transaction.TransactionId,
                Type = transaction.Type
            });
        }
        private bool IsDBStatusPending(TransactionPayment transaction)
            => transaction.Status == ActionStatus.Pending;
        private async Task<TransactionPayment> GetFromQueueTransaction(CheckPaymentTransactionResponse response)
        {
            response.IsFromQueue = true;
            response.IsSuccess = _queue.TryGet(out WaitingPaymentApproval oldestPayment);
            if (!response.IsSuccess)
                return null;
            if (oldestPayment.Expires.IsNotExpire())
            {
                response.IsSuccess = false;
                response.Delay = oldestPayment.Expires.RemainingTime();
                return null;
            }
            return await GetTransaction(oldestPayment.TransactionId);
        }
        private async Task<TransactionPayment> GetTransaction(Guid transactionId)
        {
            return await _uow.Transaction().GetAsync(transactionId);
        }
    }
}
