using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Models.DTOs.Store;
using Auth.Domain.Core.Logic.Models.Payments;

namespace Auth.Infrastructure.Logic.Read.Mappers
{
    internal class StoreMapper : IStoreMapper
    {
        public ProductDTO Map(Product from)
        {
            if (from == null) return default;
            return new()
            {
                Id = from.ProductId,
                Name = from.Name,
                Price = from.Price,
            };
        }
        public ProductItem Map(Product from, ushort amount, PaymentCurrency currency = PaymentCurrency.usd)
        {
            if (from == null) return default;
            return new(
                from.Name, from.Price, currency, amount);
        }
        public ProductSubscriptionDTO Map(ProductSubscription from)
        {
            if (from == null) return default;
            return new()
            {
                ProductId = from.ProductId,
                Type = from.Type,
            };
        }
        public OrderDTO Map(Order from)
        {
            if (from == null) return default;
            return new()
            {
                Currency = from.Currency,
                Products =  from.Products.Select(Map),
            };
        }
        public ProductOrderDTO Map(ProductOrder from)
        {
            if (from == null) return default;
            return new()
            {
                Amount = from.Amount,
                Name = from.Product.Name,
                Type = from.Product.Type,
                Price = from.Product.Price
            };
        }
    }  
}
