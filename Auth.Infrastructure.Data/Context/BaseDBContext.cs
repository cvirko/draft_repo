namespace Auth.Infrastructure.Data.Context
{
    internal class BaseDBContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLogin> UsersLogin { get; set; }
        public virtual DbSet<UserToken> UsersTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity =>
            {
                entity.HasKey(p => p.UserId);
                entity.HasMany(p => p.Logins)
                .WithOne(p => p.User).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(p => p.LoginId);
            });
            builder.Entity<UserToken>(entity =>
            {
                entity.HasKey(p => p.UserTokenId);
            });
        }
    }
}
