using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Domain.Core.Data.DBEntity.Store
{
    public class Order : TEntity
    {
        public ulong OrderId { get; set; }
        public Guid? TransactionId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<ProductOrder> Products { get; set; }
        public DateTime DateAt { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentCurrency Currency { get; set; }
    }
}
