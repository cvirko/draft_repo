using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Data.DBEntity.Store;
using Auth.Domain.Core.Data.DBEntity.Transactions;
using Order = Auth.Domain.Core.Data.DBEntity.Store.Order;

namespace Auth.Infrastructure.Data.Context
{
    internal class BaseDBContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLogin> UsersLogin { get; set; }
        public virtual DbSet<UserToken> UsersTokens { get; set; }
        public virtual DbSet<UserWallet> Wallets { get; set; }

        public virtual DbSet<TransactionPayment> TransactionPayments { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrder> ProductsOrders { get; set; }
        public virtual DbSet<ProductSubscription> ProductSubscriptions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.HasKey(p => p.UserId);
                entity
                .HasMany(p => p.Logins)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);
                entity
                .HasMany(p => p.Orders)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);
                entity
                .HasOne(p => p.Wallet)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(p => p.LoginId);
                entity.HasIndex(p => p.Email).IsUnique(); ;
            });
            builder.Entity<UserToken>(entity =>
            {
                entity.HasKey(p => new { p.UserId, p.TokenType, p.UserInfo });
            });
            builder.Entity<UserWallet>(entity =>
            {
                entity.HasKey(p => p.WalletId);
                entity.Property(p => p.Balance)
                .HasColumnType("decimal(19,4)")
                .HasPrecision(19, 4);
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasKey(p => p.OrderId);
                entity.HasMany(p => p.Products)
                .WithOne(p => p.Order).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Price)
                .HasColumnType("decimal(10,2)")
                .HasPrecision(10, 2);
                entity.HasMany(p => p.Subscriptions)
                .WithOne(p => p.Product).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(p => p.Orders)
                .WithOne(p => p.Product).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(p => new { p.OrderId , p.ProductId });
                entity.Property(p => p.Price)
                .HasColumnType("decimal(10,2)")
                .HasPrecision(10, 2);
            });
            builder.Entity<ProductSubscription>(entity =>
            {
                entity.HasKey(p => new { p.ProductId, p.Type });
            });
            
            builder.Entity<TransactionPayment>(entity =>
            {
                entity.HasKey(p => p.TransactionId);
            });
            
        }
    }
}
