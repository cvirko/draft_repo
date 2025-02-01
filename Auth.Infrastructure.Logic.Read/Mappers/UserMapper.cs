using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using System.Security.Claims;

namespace Auth.Infrastructure.Logic.Read.Mappers
{
    internal class UserMapper : IUserMapper
    {
        public UserDTO Map(User from, string avatarsURL)
        {
            if (from == null) return default;
            return new()
            {
                Role = from.Role,
                UserId = from.UserId,
                UserName = from.UserName,
                AvatarURL = Path.Combine(avatarsURL,  from.UserId.ToAvatarName())
            };
        }
        public LoginDTO Map(UserLogin from)
        {
            if (from == null) return default;
            return new()
            {
                Role = from.User.Role,
                UserId = from.UserId,
                UserName = from.User.UserName,
                LoginId = from.LoginId,
                LoginDate = DateTimeExtension.Get(),
                TokenLoginId = Guid.NewGuid(),
            };
        }
        public LoginDTO Map(SignUpInCacheCommand from, RoleType role = RoleType.User)
        {
            if (from == null) return default;
            return new()
            {
                Role = role,
                UserId = from.UserId,
                UserName = from.UserName,
                LoginId = from.LoginId,
                LoginDate = DateTimeExtension.Get(),
                TokenLoginId = Guid.NewGuid(),
            };
        }
        public LoginDTO Map(ClaimsPrincipal from)
        {
            if (from == null) return default;
            return new()
            {
                Role = from.GetAccess(),
                UserId = from.GetUserId(),
                UserName = from.GetFullName(),
                LoginId = from.GetLoginId(),
                LoginDate = from.GetDate(),
                TokenLoginId = from.GetTokenLoginId(),
            };
        }
        public LoginDTO Map(SocialData from, UserLogin login)
        {
            if (from == null) return default;
            return new()
            {
                Role = login?.User?.Role ??  RoleType.User,
                UserName = from.Name ,
                LoginId = login?.LoginId ?? default,
                LoginDate = DateTimeExtension.Get(),
                TokenLoginId = Guid.NewGuid(),
            };
        }
    }
}
