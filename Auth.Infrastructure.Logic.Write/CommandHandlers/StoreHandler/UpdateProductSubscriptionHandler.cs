using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Commands.Store;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.StoreHandler
{
    internal class UpdateProductSubscriptionHandler(IUnitOfWork uow)
        : Handler<UpdateProductSubscriptionCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        public override async Task HandleAsync(UpdateProductSubscriptionCommand command)
        {
            var subscription = await GetOrAddSubscriptionAsync(command);
            if (!string.IsNullOrEmpty(command.PaymentId) 
                && subscription.PaymentId != command.PaymentId) 
                subscription.PaymentId = command.PaymentId;
            if (subscription.IsActive != command.IsActive)
                subscription.IsActive = command.IsActive;
            await _uow.SaveAsync();
        }
        private async Task<ProductSubscription> GetOrAddSubscriptionAsync(UpdateProductSubscriptionCommand command)
        {
            var subscription = await _uow.Store()
                .GetSubscriptionAsync(command.ProductId, command.Type);
            if (subscription is not null)
                return subscription;
            subscription = new ProductSubscription
            {
                PaymentId = command.PaymentId,
                ProductId = command.ProductId,
                Type = command.Type,
                IsActive = command.IsActive
            };
            await _uow.AddAsync(subscription);
            return subscription;
        }
    }
}
