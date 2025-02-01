using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.External.Auth;
using Auth.Domain.Interface.Logic.External.Randomiz;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;

namespace Auth.Infrastructure.Logic.Read.ModelBuilders.ServicesBuilder
{
    internal class TokenBuilder(ITokenService token, IRandomService random) : ITokenBuilder
    {
        private readonly ITokenService _token = token;
        private readonly IRandomService _random = random;

        public TokenData CreateNumericToken(int length = 6)
        {
            var code = new int[length];
            for (var i = 0; i < length; i++)
                code[i] = _random.Get(9);

            return new()
            {
                TokenType = TokenType.ConfirmMail,
                Expires = DateTimeExtension.WithMinutes(_token.ConfirmationTokenExpiresTimeInMinutes),
                Token = string.Join("", code)
            };
        }
        public TokenData CreateEmailConfirmationToken(LoginDTO token, string email, TokenType type = TokenType.ConfirmMail)
        {
            return new()
            {
                TokenType = type,
                Expires = DateTimeExtension.WithMinutes(_token.ConfirmationTokenExpiresTimeInMinutes),
                Token = _token.GenerateConfirmation(token, email, type),
            };
        }
        public TokenData CreateAccessTokens(LoginDTO token)
        {
            return new()
            {
                TokenType = TokenType.Access,
                Expires = DateTimeExtension.WithMinutes(_token.AccessTokenExpiresTimeInMinutes),
                Token = _token.GenerateAccess(token),
            };
        }
        public TokenData CreateRefreshTokens(LoginDTO token, Guid id)
        {
            return new()
            {
                TokenType = TokenType.Refresh,
                Expires = DateTimeExtension.WithMinutes(_token.RefreshTokenExpiresTimeInMinutes),
                Token = _token.GenerateRefresh(token, id),
            };
        }
    }
}
