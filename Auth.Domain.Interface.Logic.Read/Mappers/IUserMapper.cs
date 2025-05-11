using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auth.Domain.Interface.Logic.Read.Mappers
{
    public interface IUserMapper : IMapper
    {
        UserDTO Map(User from, string avatarsURL);
        LoginDTO Map(UserLogin from, HttpRequest request);
        LoginDTO Map(SocialData from, UserLogin login, HttpRequest request);
        LoginDTO Map(SignUpInCacheCommand from, HttpRequest request, RoleType role = RoleType.User);
        LoginDTO Map(ClaimsPrincipal from, HttpRequest request);
    }
}
