using Auth.Infrastructure.Data.Context.Tools;

namespace Auth.Infrastructure.Data.Context
{
    internal class AuthDBContext : BaseDBContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DataSeeding.Seeding(builder);
        }
    }
}
