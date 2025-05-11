using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Infrastructure.Data.Context.Tools
{
    internal static class DataSeeding
    {
        public static void Seeding(ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("D1034B9E-AFD1-46E9-9504-AF13AB122897");
            modelBuilder.Entity<User>().HasData(
                new User(userId, RoleType.SuperAdmin, UserStatus.Active)
                {
                    CreationDate = DateTimeExtension.Get(),
                    UserName = "Admin"
                });
            modelBuilder.Entity<UserLogin>().HasData(
                new UserLogin
                {
                    LoginId = 1,
                    DateAt = DateTimeExtension.Get(),
                    Email = "admin@gmail.com",
                    UserId = userId,
                    Attempts = 5,
                    PasswordHash = "AQAAAAIAAYagAAAAEKEXeiXikqPiFSS0KIi1LkYafDhKo6HRdnZFIMqqsU7nt9KBi02s8blThOjOHKO0Ag==",
                });
            modelBuilder.Entity<UserWallet>().HasData(
                new UserWallet
                {
                    UserId = userId,
                    Balance = 9999999,
                    WalletId = 1
                });
        }
    }
}
