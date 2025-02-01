using Auth.Domain.Interface.Data.Read.Repository;
using Auth.Infrastructure.Data.Repository;

namespace Auth.Infrastructure.Data.UnitOfWork
{
    internal class UnitOfWorkRead : IUnitOfWorkRead
    {
        private readonly AuthReadDBContext _context;
        private bool disposed = false;

        private IUserRepository userRepo = default;

        public UnitOfWorkRead(AuthReadDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUserRepository Users() => userRepo ?? new UserRepository<AuthReadDBContext>(_context);

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
