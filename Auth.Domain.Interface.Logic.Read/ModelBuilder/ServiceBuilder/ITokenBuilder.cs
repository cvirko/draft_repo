using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder
{
    public interface ITokenBuilder : IBuilder
    {
        TokenData CreateEmailConfirmationToken(LoginDTO token, string email, TokenType type = TokenType.ConfirmMail);
        TokenData CreateAccessTokens(LoginDTO token);
        TokenData CreateRefreshTokens(LoginDTO token, Guid id);
        TokenData CreateNumericToken(int length = 6);
    }
}
