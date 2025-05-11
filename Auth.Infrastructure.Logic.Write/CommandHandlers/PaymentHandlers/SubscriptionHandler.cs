using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;
using Auth.Domain.Interface.Logic.External.Payments;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.PaymentHandlers
{
    internal class SubscriptionHandler(IUnitOfWork unitOfWork, 
        IPaymentUnitOfWork payment, ICommandDispatcher dispatcher)
        : Handler<SubscriptionCommand, SubscriptionResponse>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IPaymentUnitOfWork _payment = payment;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        public override async Task<SubscriptionResponse> HandleAsync(SubscriptionCommand command)
        {
            var subscription = await _uow.Store().GetSubscriptionAsync(command.ProductId,command.Type);
            var transactionId = await InitTransactionAsync(command);

            var response = await CreatePaymentAsync(
                command, subscription, transactionId);
            var order = CreateOrderAsync(command, transactionId);
            await UpdateTransactionAsync(transactionId, response.PaymentId);
            await _uow.SaveAsync();
            return response;
        }
        private async Task<Guid> InitTransactionAsync(SubscriptionCommand command)
        {
            return await _dispatcher.ProcessAsync
                (new AddPaymentTransactionCommand(command.UserId, command.Type));
        }
        private async Task UpdateTransactionAsync(Guid transactionId, string paymentId)
        {
            var transaction = await _uow.Transaction().GetAsync(transactionId);
            transaction.PaymentId = paymentId;
        }
        private async Task<SubscriptionResponse> CreatePaymentAsync(
            SubscriptionCommand command, 
            ProductSubscription subscription, 
            Guid transactionId)
        {
            SubscriptionResponse response = new();
            response.TransactionId = transactionId; 
            switch (command.Type) 
            {
                case PaymentType.Stripe_SubscriptionSchedule:
                    var subscriptionSchedule = await _payment
                        .Stripe()
                        .SubscribeAsync(
                            command.CustomerId,command.PaymentMethodId,
                            subscription.PaymentId, transactionId
                            );

                    response.PaymentId = subscriptionSchedule;
                    return response;
                default: return response;
            }
        }
        private async Task<Order> CreateOrderAsync(
            SubscriptionCommand command, Guid transactionId)
        {
            var product = await _uow.Store().GetProductAsync(command.ProductId);
            var order = new Order
            {
                DateAt = DateTime.UtcNow,
                Products = [new() {
                    Amount = 1, Price = product.Price,
                    Product = product, ProductId = command.ProductId}],
                Status = OrderStatus.Pending,
                TransactionId = transactionId,
                UserId = command.UserId,
            };
            await _uow.AddAsync(order);
            return order;
        }
    }
}
