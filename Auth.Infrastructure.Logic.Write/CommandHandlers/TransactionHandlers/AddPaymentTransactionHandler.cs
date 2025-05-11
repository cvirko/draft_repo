using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.DBEntity.Transactions;
using Auth.Domain.Core.Data.Queues;
using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Interface.Data.Read.Queues;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.TransactionHandlers
{
    internal class AddPaymentTransactionHandler(IUnitOfWork unitOfWork,
        IQueueRepository<WaitingPaymentApproval> queue, 
        IOptionsSnapshot<PaymentOptions> options)
        : Handler<AddPaymentTransactionCommand, Guid>
    {
        private readonly PaymentOptions _options = options.Value;
        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IQueueRepository<WaitingPaymentApproval> _queuePayment = queue;
        public override async Task<Guid> HandleAsync(AddPaymentTransactionCommand command)
        {
            var transaction = new TransactionPayment(
                command.UserId,Guid.CreateVersion7(),
                command.Type);
            await _uow.AddAsync(transaction);
            await _uow.SaveAsync();

            _queuePayment.Add(new(
                transaction.TransactionId, 
                transaction.Type, 
                transaction.Date.AddMinutes(_options.ExpirePendingInMinutes)));
            return transaction.TransactionId;
        }
    }
}
