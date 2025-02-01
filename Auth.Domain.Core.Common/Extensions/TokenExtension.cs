using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Common.Enums;
using System.Security.Claims;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class TokenExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal User)
        {
            var userId = User.FindFirst(AuthConsts.CLAIM_TYPE_NAME_USER_ID)?.Value;
            if (userId == null) return Guid.Empty;
            return Guid.Parse(userId);
        }
        public static RoleType GetAccess(this ClaimsPrincipal User)
        {
            var role = User.FindFirst(AuthConsts.CLAIM_TYPE_NAME_ACCESS)?.Value;
            if (role == null) return RoleType.Guest;
            return Enum.Parse<RoleType>(role);
        }
        public static DateTime GetDate(this ClaimsPrincipal User)
        {
            var date = User.FindFirst(AuthConsts.CLAIM_TYPE_NAME_LOGIN_DATE)?.Value;
            if (date == null) return DateTimeExtension.Get();
            return DateTime.Parse(date);
        }
        public static Guid GetTokenLoginId(this ClaimsPrincipal User)
        {
            var tokenLoginId = User.FindFirst(AuthConsts.CLAIM_TYPE_NAME_LOGIN_TOKENID)?.Value;
            if (tokenLoginId == null) return Guid.Empty;
            return Guid.Parse(tokenLoginId);
        }
        public static ulong GetLoginId(this ClaimsPrincipal User)
        {
            var loginId = User.FindFirst(AuthConsts.CLAIM_TYPE_NAME_LOGINID)?.Value;
            if (loginId == null) return 0;
            return Convert.ToUInt64(loginId);
        }
        public static string GetUserEmail(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }
        public static string GetFullName(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static string GetRole(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
        public static string GetTokenId(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
