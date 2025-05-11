using Auth.Domain.Core.Logic.Commands.Payments;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.PaymentHandlers
{
    internal class ConfirmPaymentActionHandler(ICommandDispatcher dispacher,
        IUnitOfWork uow)
        : Handler<ConfirmPaymentActionCommand>
    {
        private readonly ICommandDispatcher _dispacher = dispacher;
        private readonly IUnitOfWork _uow = uow;
        public override async Task HandleAsync(ConfirmPaymentActionCommand command)
        {

            await UpdateDBTransactionStatusAsync(command);

            switch (command.Type)
            {
                case PaymentType.Stripe_PaymentIntent:
                case PaymentType.Stripe_Charge:
                case PaymentType.Stripe_Checkout_Session:
                case PaymentType.Stripe_Invoice:
                    await ConfirmOrderAsync(command); break;
                case PaymentType.Stripe_ProductPrice:
                    await ConfirmUpdatingSubscriptionAsync(command); break;
                case PaymentType.Stripe_SubscriptionSchedule:
                    //add
                default: 
                    throw new NotImplementedException(command.Type.ToString());
            }
        }
        private async Task UpdateDBTransactionStatusAsync(ConfirmPaymentActionCommand command)
        {
            var transaction = await _uow.Transaction().GetAsync(command.TransactionId);

            transaction.Status =
                (command.Status == ActionStatus.Pending)
                ? ActionStatus.Expired
                : command.Status;
            await _uow.SaveAsync();
        }
        private async Task ConfirmUpdatingSubscriptionAsync(ConfirmPaymentActionCommand command)
        {
            if (command.Status != ActionStatus.Successed &&
                command.Status != ActionStatus.Updated)
                return;
            var subscription = await _uow.Store().GetSubscriptionAsync(command.PaymentId, command.Type);
            if (subscription == null) return;
            subscription.IsConfirmed = true;
            await _uow.SaveAsync();
        }
        private async Task ConfirmOrderAsync(ConfirmPaymentActionCommand command)
        {
            var order = await _uow.Store().GetOrderAsync(command.TransactionId);
            order.Status = Convert(command.Status);
            await _uow.SaveAsync();

            if (command.Status != ActionStatus.Successed &&
                command.Status != ActionStatus.Updated)
                return;

            var total = order.Products.Sum(p => p.Amount * p.Price);
            await _dispacher.ProcessAsync(
                new UpdateWalletCommand(order.UserId, total));
        }
        private OrderStatus Convert(ActionStatus actionStatus)
        {
            switch (actionStatus)
            {
                case ActionStatus.None:
                    return OrderStatus.Fail;
                case ActionStatus.Canceled:
                    return OrderStatus.Canceled;
                case ActionStatus.Successed:
                case ActionStatus.Updated:
                    return OrderStatus.Paid;
                case ActionStatus.Pending:
                case ActionStatus.Expired:
                    return OrderStatus.Expired;
                default: return OrderStatus.Fail;
            }
        }
    }
}
