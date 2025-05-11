using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Interface.Logic.External.Payments;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.PaymentHandlers
{
    internal class CreateProductSubscriptionHandler(IUnitOfWork uow,
        IPaymentUnitOfWork payment, ICommandDispatcher dispatcher)
        : Handler<CreateProductSubscriptionCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IPaymentUnitOfWork _payment = payment;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        public override async Task HandleAsync(CreateProductSubscriptionCommand command)
        {
            var transactionId = await InitTransactionAsync(command);
            var  product = await _uow.Store().GetProductAsync(command.ProductId);
            string paymentId = "";
            switch (command.Type)
            {
                case PaymentType.Stripe_ProductPrice:
                    paymentId = await StripeAsync(product, transactionId, command.Currency);
                    break;
                default: throw new NotImplementedException(command.Type.ToString());   
            }
         
            await UpdateSubscriptionAsync(command, paymentId);
            await UpdateTransactionAsync(transactionId, paymentId);
            await _uow.SaveAsync();
        }
        private async Task<Guid> InitTransactionAsync(CreateProductSubscriptionCommand command)
        {
            return await _dispatcher.ProcessAsync
                            (new AddPaymentTransactionCommand(command.UserId, command.Type));
        }
        private async Task UpdateTransactionAsync(Guid transactionId, string paymentId)
        {
            var transaction = await _uow.Transaction().GetAsync(transactionId);
            transaction.PaymentId = paymentId;
        }
        private async Task<string> StripeAsync(Product product, Guid transactionId, PaymentCurrency currency)
        {
            var productPaymentId = await _payment.Stripe()
                .CreateProductAsync(product.Name, transactionId);
            var priceInCents = Convert.ToInt64(product.Price * 100);
            var priceId = await _payment.Stripe()
                .CreateSubscribtionPriceAsync(productPaymentId, 
                priceInCents, currency, transactionId);
            return priceId;
        }
        private async Task UpdateSubscriptionAsync(CreateProductSubscriptionCommand command, string paymentId)
        {
            var subscription = await _uow.Store()
                .GetSubscriptionAsync(command.ProductId, command.Type);
            if (subscription is null)
            {
                subscription = new ProductSubscription
                {
                    ProductId = command.ProductId,
                    Type = command.Type,
                    IsActive = false,
                    PaymentId = paymentId
                };
                await _uow.AddAsync(subscription);
                return;
            }
            subscription.PaymentId = paymentId;
            subscription.IsConfirmed = false;
            subscription.IsActive = false;
        }
    }
}
