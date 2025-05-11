using Auth.Domain.Core.Data.DBEntity.Transactions;
using Auth.Domain.Core.Data.Queues;
using Auth.Domain.Interface.Data.Read.Repository;

namespace Auth.Infrastructure.Data.Repository
{
    internal class TransactionRepository<T>(T dbContext) 
        : ITransactionRepository where T : BaseDBContext
    {
        private readonly T _context = dbContext;

        public IAsyncEnumerable<WaitingPaymentApproval> GetPendingAsync(double pendingExpireInMinutes)
        {
            return _context.TransactionPayments
                .Where(p => p.Status == ActionStatus.Pending)
                .Select(p => new WaitingPaymentApproval
                    (p.TransactionId, p.Type, p.Date.AddMinutes(pendingExpireInMinutes), 1))
                .AsAsyncEnumerable();
        }
        public async Task<bool> IsPendingAsync(Guid transactionId)
        {
            return await _context.TransactionPayments
                .AnyAsync(p => p.TransactionId == transactionId &&
                               p.Status == ActionStatus.Pending);
        }
        public async Task<TransactionPayment> GetAsync(Guid transactionId)
        {
            return await _context.TransactionPayments.FindAsync(transactionId);
        }
    }
}
