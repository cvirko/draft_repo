using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Data.Extensions;
using Auth.Domain.Core.Logic.Commands.Payments;
using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;
using Auth.Domain.Core.Logic.Models.Payments;
using Auth.Domain.Interface.Logic.External.Payments;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.PaymentHandlers
{
    internal class PayForOrderHandler(IUnitOfWork unitOfWork, 
        IPaymentUnitOfWork payment, ICommandDispatcher dispatcher)
        : Handler<PayForOrderCommand, PayForOrderResponse>
    {
        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IPaymentUnitOfWork _payment = payment;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        public override async Task<PayForOrderResponse> HandleAsync(PayForOrderCommand command)
        {
            var transactionId = await InitTransactionAsync(command);
            var order = await GetAndUpdateOrderAsync(command, transactionId);
            UpdatePrice(order);
            var response = await CreatePaymentAsync(command, order);
            await UpdateTransactionAsync(transactionId, response.PaymentId);
            await _uow.SaveAsync();
            return response;
        }
        private async Task<Guid> InitTransactionAsync(PayForOrderCommand command)
        {
            return await _dispatcher.ProcessAsync
                            (new AddPaymentTransactionCommand(command.UserId, command.Type));
        }
        private async Task UpdateTransactionAsync(Guid transactionId, string paymentId)
        {
            var transaction = await _uow.Transaction().GetAsync(transactionId);
            transaction.PaymentId = paymentId;
        }
        private async Task<PayForOrderResponse> CreatePaymentAsync(PayForOrderCommand command, Order order)
        {
            PayForOrderResponse response = new();
            response.TransactionId = order.TransactionId.Value;
            switch (command.Type) 
            {
                case PaymentType.Stripe_PaymentIntent:
                    var paymentIntentResponse = await _payment
                        .Stripe()
                        .PayAsync(
                            ConvertToGeneral(order, command), 
                            order.TransactionId.Value
                            );

                    response.UserSecret = paymentIntentResponse.ClientSecret;
                    response.PaymentId = paymentIntentResponse.Id;
                    return response;
                case PaymentType.Stripe_Checkout_Session:
                    string checkoutId = await _payment
                        .Stripe()
                        .CreateCheckoutSessionAsync(
                            ConvertToItems(order, command).ToArray(),
                            order.TransactionId.Value
                    );

                    response.PaymentId = checkoutId;
                    return response;
                default: return response;
            }
        }
        private IEnumerable<ProductItem> ConvertToItems(Order order, PayForOrderCommand command)
        {
            return order.Products.Select(p =>
                new ProductItem
                (
                    p.Product.Name,
                    p.Price,
                    command.Currency,
                    p.Amount
                ));
        }
        private ProductItem ConvertToGeneral(Order order, PayForOrderCommand command)
        {
            return new ProductItem
                (
                    string.Format("Order {0}", order.OrderId),
                    order.GetTotalPrice(),
                    command.Currency
                );
        }
        private void UpdatePrice(Order order)
        {
            foreach (var productOrder in order.Products)
            {
                productOrder.Price = productOrder.Product.Price;
            }
        }
        private async Task<Order> GetAndUpdateOrderAsync(PayForOrderCommand command, Guid transactionId)
        {
            var order = await _uow.Store().GetCollectingOrderWithProductsAsync(command.UserId);

            order.Status = OrderStatus.Pending;
            order.TransactionId = transactionId;
            order.Currency = command.Currency;
            return order;
        }
    }
}
