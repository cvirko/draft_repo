using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Logic.Models.DTOs.Store;
using Auth.Domain.Core.Logic.Models.Payments;

namespace Auth.Domain.Interface.Logic.Read.Mappers
{
    public interface IStoreMapper : IMapper
    {
        OrderDTO Map(Order from);
        public ProductDTO Map(Product from);
        public ProductSubscriptionDTO Map(ProductSubscription from);
        ProductItem Map(Product from, ushort amount, PaymentCurrency currency = PaymentCurrency.usd);
    }
}
