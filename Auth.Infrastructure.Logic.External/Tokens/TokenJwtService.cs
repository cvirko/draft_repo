using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Interface.Logic.External.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Infrastructure.Logic.External.Tokens
{
    internal class TokenJwtService(IOptionsSnapshot<TokenOptions> option) : ITokenService
    {
        private readonly TokenOptions _option = option.Value;
        public float AccessTokenExpiresTimeInMinutes => _option.AccessTokenExpiresTimeInMinutes;
        public float RefreshTokenExpiresTimeInMinutes => _option.RefreshTokenExpiresTimeInMinutes;
        public float ConfirmationTokenExpiresTimeInMinutes => _option.ConfirmationTokenExpiresTimeInMinutes;
        public string GenerateRefresh(LoginDTO user, Guid identifier)
        {
            Claim[] claims = [
                new (ClaimTypes.Role, TokenType.Refresh.ToString()),
                new (ClaimTypes.NameIdentifier, identifier.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_USER_ID, user.UserId.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_LOGINID, user.LoginId.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_ACCESS, user.Role.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_LOGIN_TOKENID, user.UserLoginInfo.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_LOGIN_DATE, user.LoginDate.ToString()),
                new (ClaimTypes.Name, user.UserName)
                ];
            DateTime? expires = DateTime.Now.AddMinutes(_option.RefreshTokenExpiresTimeInMinutes);
            return GenerateToken(expires, claims);
        }
        public string GenerateAccess(LoginDTO user, float? expiresTimeInMinutes = null)
        {
            Claim[] claims = [
                new (ClaimTypes.Role, user.Role.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_USER_ID, user.UserId.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_ACCESS, user.Role.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_LOGINID, user.LoginId.ToString()),
                new (ClaimTypes.Name, user.UserName)
                ];
            DateTime? expires = DateTime.Now.AddMinutes(expiresTimeInMinutes ?? _option.AccessTokenExpiresTimeInMinutes);
            return GenerateToken(expires, claims);
        }

        public string GenerateConfirmation(LoginDTO user, string email, TokenType type, float? expiresTimeInMinutes = null)
        {
            Claim[] claims = [
                new (ClaimTypes.Role, type.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_USER_ID, user.UserId.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_ACCESS, user.Role.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_LOGINID, user.LoginId.ToString()),
                new (AuthConsts.CLAIM_TYPE_NAME_LOGIN_TOKENID, user.UserLoginInfo.ToString()),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, email)
                ];
            DateTime? expires = DateTime.Now.AddMinutes(expiresTimeInMinutes ?? _option.ConfirmationTokenExpiresTimeInMinutes);
            return GenerateToken(expires, claims);
        }

        private string GenerateToken(DateTime? expires, IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(_option.TokenSecret.ToByteArray());

            var creds = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_option.ValidIssuer,
                _option.ValidAudience,
                claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
