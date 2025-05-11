using Auth.Domain.Core.Common.Enums;

namespace Auth.Domain.Interface.Logic.External.Payments
{
    public interface IPaymentUnitOfWork
    {
        public Task CreateWebhookConfigurationsAsync();
        public Task<ActionStatus> GetTransactionStatusAsync(string id, PaymentType type);
        public IStripePayment Stripe();
    }
}
