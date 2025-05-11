using Auth.Domain.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity.Store
{
    public class ProductSubscription : TEntity
    {
        public uint ProductId { get; set; }
        public Product Product { get; set; }
        public PaymentType Type { get; set; }
        [StringLength(100)]
        public string PaymentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsConfirmed { get; set; }
        
    }
}
