using Auth.Domain.Interface.Data.Read.Repository;
using Auth.Infrastructure.Data.Repository;

namespace Auth.Infrastructure.Data.UnitOfWork
{
    internal class UnitOfWorkRead : IUnitOfWorkRead
    {
        private readonly AuthReadDBContext _context;
        private bool disposed = false;

        private IUserRepository userRepo = default;
        private ITransactionRepository transactionRepo = default;
        private IStoreRepository storeRepo = default;
        public UnitOfWorkRead(AuthReadDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUserRepository Users()
        {
            if (userRepo == null)
                userRepo = new UserRepository<AuthReadDBContext>(_context);
            return userRepo;
        }
        public ITransactionRepository Transaction()
        {
            if (transactionRepo == null)
                transactionRepo = new TransactionRepository<AuthReadDBContext>(_context);
            return transactionRepo;
        }
        public IStoreRepository Store()
        {
            if (storeRepo == null)
                storeRepo = new StoreRepository<AuthReadDBContext>(_context);
            return storeRepo;
        }
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
    }
}
