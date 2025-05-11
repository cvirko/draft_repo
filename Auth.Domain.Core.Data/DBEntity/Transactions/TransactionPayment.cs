using Auth.Domain.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity.Transactions
{
    public class TransactionPayment : BaseTransaction<PaymentType>
    {
        public TransactionPayment() {}
        public TransactionPayment(
            Guid userId, Guid transactionId,
            PaymentType type) 
            : base(userId, transactionId, type)
        { 
        }
        [StringLength(128)]
        public string PaymentId { get; set; }
    }
}
