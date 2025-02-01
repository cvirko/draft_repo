namespace Auth.Infrastructure.Data.Context
{
    internal class AuthReadDBContext : BaseDBContext
    {
        public AuthReadDBContext(DbContextOptions<AuthReadDBContext> options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
