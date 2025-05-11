using Auth.Domain.Core.Logic.Models.DTOs.Store;
using Auth.Domain.Core.Logic.Models.Payments;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.StoreBuilder;

namespace Auth.Infrastructure.Logic.Read.ModelBuilders.StoreBuilder
{
    internal class StoreBuilder(IUnitOfWorkRead uow, IStoreMapper mapper) : IStoreBuilder
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly IStoreMapper _mapper = mapper;

        public async Task<OrderDTO> GetOrderAsync(Guid userId)
        {
            var order = await _uow.Store().GetCollectingOrderWithProductsAsync(userId);
            return _mapper.Map(order);
        }
        public async Task<ProductDTO[]> GetProductsAsync()
        {
            var products = await _uow.Store().GetAciveProductsAsync();
            return products.Select(_mapper.Map).ToArray();
        }
        public async Task<ProductDTO> GetProductAsync(uint productId)
        {
            var product = await _uow.Store().GetProductAsync(productId);
            return _mapper.Map(product);
        }
        public async Task<ProductItem[]> GetProductsAsync(Dictionary<uint,ushort> productIds)
        {
            var products = await _uow.Store().GetProductsAsync(productIds.Keys);
            return products.Select(p => 
                _mapper.Map(p, productIds[p.ProductId]))
                .ToArray();
        }
        public async Task<ProductSubscriptionDTO[]> GetSubscriptionsAsync(uint productId)
        {
            var subscriptions = await _uow.Store().GetSubscriptionsAsync(productId);
            return subscriptions.Select(_mapper.Map).ToArray();
        }
        public async Task<string> GetSubscriptionIdAsync(uint productId, PaymentType type)
        {
            var subscription = await _uow.Store().GetSubscriptionAsync(productId, type);
            return subscription.PaymentId;
        }
        public async Task<bool> IsExistSubscriptionsAsync(uint productId, PaymentType type)
        {
            return null != await _uow.Store().GetSubscriptionAsync(productId, type);
        }
    }
}
