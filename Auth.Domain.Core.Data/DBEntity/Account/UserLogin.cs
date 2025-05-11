using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity.Account
{
    public class UserLogin : AccessAttempt
    {
        public ulong LoginId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? BanExpireDate { get; set; }

        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(128)]
        public string PasswordHash { get; set; }
    }
}
