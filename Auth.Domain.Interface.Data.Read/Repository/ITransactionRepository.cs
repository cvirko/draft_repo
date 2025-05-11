using Auth.Domain.Core.Data.DBEntity.Transactions;
using Auth.Domain.Core.Data.Queues;

namespace Auth.Domain.Interface.Data.Read.Repository
{
    public interface ITransactionRepository
    {
        IAsyncEnumerable<WaitingPaymentApproval> GetPendingAsync(double pendingExpireInMinutes);
        Task<bool> IsPendingAsync(Guid transactionId);
        Task<TransactionPayment> GetAsync(Guid transactionId);
    }
}
