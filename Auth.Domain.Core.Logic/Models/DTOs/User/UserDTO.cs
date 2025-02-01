namespace Auth.Domain.Core.Logic.Models.DTOs.User
{
    public class UserDTO
    {
        public UserDTO()
        {
                
        }
        public UserDTO(Guid id, string name, RoleType role)
        {
            UserId = id;
            UserName = name;
            Role = role;
        }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public RoleType Role { get; set; }
        public string AvatarURL { get; set; }
    }
}
