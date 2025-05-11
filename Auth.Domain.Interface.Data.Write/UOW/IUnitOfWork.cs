using Auth.Domain.Core.Data;
using Auth.Domain.Interface.Data.Read.UOW;

namespace Auth.Domain.Interface.Data.Write.UOW
{
    public interface IUnitOfWork : IUnitOfWorkRead
    {
        Task AddAsync<T>(params T[] models) where T : TEntity;
        void Add<T>(params T[] models) where T : TEntity;
        void Remove<T>(params T[] models) where T : TEntity;
        int Save();
        Task<int> SaveAsync();
        Task<int> UpdateBalanceAsync(Guid userId, decimal amount, CancellationToken token = default);
        Task RemoveTokensBeforeAsync(DateTime date, CancellationToken token);
    }
}
