using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using System.Security.Claims;

namespace Auth.Domain.Interface.Logic.Read.Mappers
{
    public interface IUserMapper : IMapper
    {
        UserDTO Map(User from, string avatarsURL);
        LoginDTO Map(UserLogin from);
        LoginDTO Map(SocialData from, UserLogin login);
        LoginDTO Map(SignUpInCacheCommand from, RoleType role = RoleType.User);
        LoginDTO Map(ClaimsPrincipal from);
    }
}
