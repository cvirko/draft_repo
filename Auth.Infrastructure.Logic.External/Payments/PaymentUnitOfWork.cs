using Auth.Domain.Interface.Logic.External.Payments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Logic.External.Payments
{
    internal class PaymentUnitOfWork(ILogger<PaymentUnitOfWork> logger, 
        IServiceProvider services) : IPaymentUnitOfWork
    {
        private readonly ILogger<PaymentUnitOfWork> _logger = logger;
        private readonly IServiceProvider _services = services;
        private IStripePayment _stripe;
        public IStripePayment Stripe()
        {
            if (_stripe is null)
            {
                _stripe = _services.GetRequiredService<IStripePayment>();
            }
            return _stripe;
        }

        public async Task CreateWebhookConfigurationsAsync()
        {
            await Stripe().CreateWebhookEndpointAsync();
        }
        public async Task<ActionStatus> GetTransactionStatusAsync(string id, PaymentType type)
        {
            if (string.IsNullOrEmpty(id))
                return ActionStatus.Fail;
            if (type.ToString().Contains("stripe", StringComparison.OrdinalIgnoreCase))
                return await Stripe().GetOperationStatusAsync(id, type);

            _logger.LogError("Transaction type:{0} not found", type);
            return ActionStatus.None;
        }
    }
}
