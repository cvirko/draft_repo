using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity
{
    public class UserLogin : AccessAttempt
    {
        public ulong LoginId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? BanExpireDate { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string PasswordHash { get; set; }
    }
}
