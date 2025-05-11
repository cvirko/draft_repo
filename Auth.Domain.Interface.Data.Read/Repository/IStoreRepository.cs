using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Data.DBEntity.Store;

namespace Auth.Domain.Interface.Data.Read.Repository
{
    public interface IStoreRepository
    {
        Task<bool> IsExistProductNameAsync(string name);
        Task<bool> IsExistProductAsync(uint productId);
        Task<bool> IsExistCollectingOrderAsync(Guid userId);
        Task<bool> IsExistAciveProductsAsync(IEnumerable<uint> productIds);
        Task<Product[]> GetAciveProductsAsync();
        Task<Product> GetProductAsync(uint productId);
        Task<Product[]> GetProductsAsync(IEnumerable<uint> productIds);
        Task<ProductSubscription[]> GetSubscriptionsAsync(uint productId);
        Task<ProductSubscription> GetSubscriptionAsync(uint productId, PaymentType type);
        Task<ProductSubscription> GetSubscriptionAsync(string paymentId, PaymentType type);
        Task<Order> GetOrderAsync(Guid transactionId);
        Task<Order> GetCollectingOrderAsync(Guid userId);
        Task<Order> GetCollectingOrderWithProductsAsync(Guid userId);
        Task<ProductOrder[]> GetOrderProductsAsync(ulong orderId);
    }
}
