using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Data.DBEntity.Account;

namespace Auth.Domain.Core.Data.Extensions
{
    public static class UserExtension
    {
        public static UserStatus GetStatus(this User user) =>
            user.IsTimeLocked()
            ? UserStatus.TimeLock
            : user.Status;
        public static bool IsTimeLocked(this User user)
        {
            if (user.Status == UserStatus.Banned) return false;
            return user.BanExpireDate.HasValue &&
                user.BanExpireDate.Value > DateTimeExtension.Get();
        }
        public static void SetBan(this User user, float? expireInMinutes)
        {
            if (expireInMinutes.HasValue)
            {
                user.BanExpireDate = DateTimeExtension.WithMinutes(expireInMinutes.Value);
                return;
            }
            user.Status = UserStatus.Banned;
        }
        public static bool IsTimeLocked(this UserLogin login)
            => login.BanExpireDate.HasValue &&
                login.BanExpireDate.Value > DateTimeExtension.Get();
        public static void SetBan(this UserLogin login, float expireInMinutes)
            => login.BanExpireDate = DateTimeExtension.WithMinutes(expireInMinutes);

        public static bool IsHaveAttempts(this AccessAttempt model)
            => model.Attempts > 0;
    }
}
