using Auth.Domain.Core.Common.Enums;
using Auth.Domain.Core.Logic.Models.DTOs.User;

namespace Auth.Domain.Interface.Logic.External.Auth
{
    public interface ITokenService
    {
        float RefreshTokenExpiresTimeInMinutes { get; }
        float AccessTokenExpiresTimeInMinutes { get; }
        float ConfirmationTokenExpiresTimeInMinutes { get; }
        string GenerateConfirmation(LoginDTO user, string email, TokenType type, float? expiresTimeInMinutes = null);
        string GenerateAccess(LoginDTO user, float? expiresTimeInMinutes = null);
        string GenerateRefresh(LoginDTO user, Guid identifier);
    }
}
