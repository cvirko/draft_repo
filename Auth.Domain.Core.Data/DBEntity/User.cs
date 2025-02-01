using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity
{
    public class User : TEntity
    {
        public User(Guid userId, RoleType role, UserStatus status = UserStatus.Active)
        {
            UserId = userId;
            Role = role;
            Status = status;
            CreationDate = DateTimeExtension.Get();
        }
        public Guid UserId { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreationDate { get; init; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? BanExpireDate { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        public RoleType Role { get; set; }
        public IEnumerable<UserLogin> Logins { get; set; }

        public UserStatus GetStatus() => (Status != UserStatus.TimeLock  
            || BanExpireDate.Value > DateTimeExtension.Get()) 
            ? Status
            : UserStatus.Active;
        public void SetBan(float? expireInMinutes)
        {
            if (!expireInMinutes.HasValue)
            {
                Status = UserStatus.Banned;
                return;
            }
            Status = UserStatus.TimeLock;
            BanExpireDate = DateTimeExtension.WithMinutes(expireInMinutes.Value);

        }
    }
}
