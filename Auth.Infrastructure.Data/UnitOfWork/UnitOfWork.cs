using Auth.Domain.Core.Data;
using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Interface.Data.Read.Repository;
using Auth.Infrastructure.Data.Repository;
using Microsoft.Data.SqlClient;

namespace Auth.Infrastructure.Data.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDBContext _context;
        private bool disposed = false;

        private IUserRepository userRepo = default;
        private ITransactionRepository transactionRepo = default;
        private IStoreRepository storeRepo = default;
        public UnitOfWork(AuthDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUserRepository Users()
        {
            if (userRepo == null)
                userRepo = new UserRepository<AuthDBContext>(_context);
            return userRepo;
        }
        public ITransactionRepository Transaction()
        {
            if (transactionRepo == null)
                transactionRepo = new TransactionRepository<AuthDBContext>(_context);
            return transactionRepo;
        }
        public IStoreRepository Store()
        {
            if (storeRepo == null)
                storeRepo = new StoreRepository<AuthDBContext>(_context);
            return storeRepo;
        }
        public async Task AddAsync<T>(params T[] models) where T : TEntity
        {
            if (IsEmpty(models)) return;
            await _context.AddRangeAsync(models);
        }
        public void Add<T>(params T[] models) where T : TEntity
        {
            if (IsEmpty(models)) return;
            _context.AddRange(models);
        }
        public void Remove<T>(params T[] models) where T : TEntity
        {
            if (IsEmpty(models)) return;
            _context.RemoveRange(models);
        }
        public async Task RemoveTokensBeforeAsync(DateTime date, CancellationToken token)
        {
            const string query = @$"DELETE [dbo].[{nameof(AuthDBContext.UsersTokens)}] 
                    Where {nameof(UserToken.DateAt)} <= '@date';";
            SqlParameter[] parameters = 
                [new("@date",date.ToString("yyyy-MM-dd HH:mm:ss"))];
            await _context.Database.ExecuteSqlRawAsync(query, parameters, token);
        }
        public async Task<int> UpdateBalanceAsync(Guid userId, decimal amount, CancellationToken token = default)
        {
            const string query = @$"
                EXEC [dbo].P_CreatePayment @userId, @amount";

            SqlParameter[] parameters = [
                new("@amount",amount),
                new("@userId",userId)
                ];
            return await _context.Database.ExecuteSqlRawAsync(query, parameters, token);
        }
        public int Save() => _context.SaveChanges();
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool IsEmpty<T>(params T[] models) where T : TEntity
        {
            if (models == null || models.Length == 0) return true;
            if (models[0] == null) return true;
            return false;
        }
    }
}
