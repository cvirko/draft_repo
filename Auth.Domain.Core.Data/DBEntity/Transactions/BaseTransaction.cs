using Auth.Domain.Core.Common.Enums;

namespace Auth.Domain.Core.Data.DBEntity.Transactions
{
    public abstract class BaseTransaction<T> : TEntity
    {
        protected BaseTransaction() {}
        protected BaseTransaction(Guid userId, Guid transactionId, T type)
        {
            TransactionId = transactionId;
            UserId = userId;
            Type = type;
        }
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public ActionStatus Status { get; set; } = ActionStatus.Pending;
        public T Type { get; set; }
    }
}
