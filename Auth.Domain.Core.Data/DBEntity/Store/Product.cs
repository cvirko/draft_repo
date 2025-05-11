using Auth.Domain.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity.Store
{
    public class Product : TEntity
    {
        public uint ProductId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductSubscription> Subscriptions { get; set; }
        public IEnumerable<ProductOrder> Orders { get; set; }
        
    }
}
