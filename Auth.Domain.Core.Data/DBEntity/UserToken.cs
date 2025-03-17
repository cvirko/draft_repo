using Auth.Domain.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Core.Data.DBEntity
{
    public class UserToken : AccessAttempt
    {
        public Guid UserId { get; set; }
        public TokenType TokenType { get; set; }
        [StringLength(100)]
        public string UserInfo { get; set; }
        [StringLength(100)]
        public string Token { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
