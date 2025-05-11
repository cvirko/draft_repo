using Auth.Domain.Core.Data.DBEntity.Store;

namespace Auth.Domain.Core.Data.Extensions
{
    public static class OrderExtension
    {
        public static decimal GetTotalPrice(this Order order)
            => order.Products.Sum(p => p.Price * p.Amount);
    }
}
