using Auth.Domain.Core.Logic.Models.DTOs.Store;
using Auth.Domain.Core.Logic.Models.Payments;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.StoreBuilder
{
    public interface IStoreBuilder : IBuilder
    {
        Task<OrderDTO> GetOrderAsync(Guid userId);
        public Task<ProductDTO[]> GetProductsAsync();
        Task<ProductDTO> GetProductAsync(uint productId);
        Task<ProductSubscriptionDTO[]> GetSubscriptionsAsync(uint productId);
        Task<ProductItem[]> GetProductsAsync(Dictionary<uint, ushort> productIds);
        Task<string> GetSubscriptionIdAsync(uint productId, PaymentType type);
        Task<bool> IsExistSubscriptionsAsync(uint productId, PaymentType type);
    }
}
