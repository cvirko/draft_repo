namespace Auth.Domain.Core.Logic.Models.DTOs.User
{
    public class LoginDTO
    {
        public ulong LoginId { get; set; }
        public Guid UserId { get; set; }
        public string UserLoginInfo { get; set; }
        public DateTime LoginDate { get; set; }
        public string UserName { get; set; }
        public RoleType Role { get; set; }
    }
}
