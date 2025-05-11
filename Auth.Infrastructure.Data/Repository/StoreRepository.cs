using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Interface.Data.Read.Repository;

namespace Auth.Infrastructure.Data.Repository
{
    internal class StoreRepository<T>(T dbContext) : IStoreRepository where T : BaseDBContext
    {
        private readonly T _context = dbContext;

        public async Task<bool> IsExistProductNameAsync(string name)
        {
            return await _context.Products
                .AnyAsync(p => p.Name.Equals(name));
        }
        public async Task<bool> IsExistProductAsync(uint productId)
        {
            return await _context.Products.AnyAsync(p =>
                p.ProductId == productId);
        }
        public async Task<bool> IsExistAciveProductsAsync(IEnumerable<uint> productIds)
        {
            return await _context.Products.AnyAsync(p =>
                p.IsActive &&
                productIds.Contains(p.ProductId));
        }
        public async Task<bool> IsExistCollectingOrderAsync(Guid userId)
        {
            return await _context.Orders
                .Where(p => p.UserId == userId &&
                            p.Status == OrderStatus.Collecting)
                .AnyAsync();
        }
        public async Task<Product[]> GetAciveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .ToArrayAsync();
        }
        public async Task<Product> GetProductAsync(uint productId)
        {
            return await _context.Products.FindAsync(productId);
        }
        public async Task<Product[]> GetProductsAsync(IEnumerable<uint> productIds)
        {
            return await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToArrayAsync();
        }
        public async Task<ProductSubscription[]> GetSubscriptionsAsync(uint productId)
        {
            return await _context.ProductSubscriptions
                .Where(p => p.ProductId == productId && p.IsActive)
                .ToArrayAsync();
        }
        public async Task<ProductSubscription> GetSubscriptionAsync(uint productId, PaymentType type)
        {
            return await _context.ProductSubscriptions.FindAsync(productId, type);
        }
        public async Task<ProductSubscription> GetSubscriptionAsync(string paymentId, PaymentType type)
        {
            return await _context.ProductSubscriptions
                .Where(p => 
                p.PaymentId == paymentId && 
                p.Type == type &&
                p.IsConfirmed == false)
                .FirstOrDefaultAsync();
        }
        public async Task<Order> GetOrderAsync(Guid transactionId)
        {
            return await _context.Orders
                .Where(p => p.TransactionId == transactionId)
                .Include(p => p.Products)
                .FirstOrDefaultAsync();
        }
        public async Task<Order> GetCollectingOrderWithProductsAsync(Guid userId)
        {
            return await _context.Orders
                .Where(p => p.UserId == userId &&
                            p.Status == OrderStatus.Collecting)
                .Include(p => p.Products).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync();
        }
        public async Task<Order> GetCollectingOrderAsync(Guid userId)
        {
            return await _context.Orders
                .Where(p => p.UserId == userId &&
                            p.Status == OrderStatus.Collecting)
                .FirstOrDefaultAsync();
        }
        public async Task<ProductOrder[]> GetOrderProductsAsync(ulong orderId)
        {
            return await _context.ProductsOrders
                .Where(p => p.OrderId == orderId)
                .OrderBy(p => p.ProductId)
                .ToArrayAsync();
        }
    }
}
