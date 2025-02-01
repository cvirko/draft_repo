namespace Auth.Domain.Core.Logic.Models.DTOs.User
{
    public class LoginDTO
    {
        public Guid UserId { get; set; }
        public Guid TokenLoginId { get; set; }
        public ulong LoginId { get; set; }
        public DateTime LoginDate { get; set; }
        public string UserName { get; set; }
        public RoleType Role { get; set; }
    }
}
