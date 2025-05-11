using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Logic.Models.Payments;
using Microsoft.AspNetCore.Http;

namespace Auth.Domain.Interface.Logic.External.Payments
{
    public interface IStripePayment
    {
        Task CreateWebhookEndpointAsync();
        Task<PaymentValidationResult> ValidateWebHookAsync(HttpRequest request);
        Task<ActionStatus> GetOperationStatusAsync(string id, PaymentType type);

        Task<(string ClientSecret, string Id)> PayAsync(ProductItem item, Guid transactionId);
        Task<string> CreateCheckoutSessionAsync(ProductItem[] items, Guid transactionId);
        Task<string> CreateProductAsync(string name, Guid transactionId);
        Task<string> CreateSubscribtionPriceAsync(string productId, long priceInCents, PaymentCurrency currency, Guid transactionId, string interval = "month");
        Task<string> SubscribeAsync(string customerId, string paymentMethodId, string priceId, Guid transactionId);
        Task<string> SubscribeUpdateAsync(string subscribeId, string priceId, string subscribItemId, Guid transactionId);
        Task<string> SubscribeCancelAsync(string subscribeId, Guid transactionId);
    }
}
