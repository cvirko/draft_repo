using Auth.Domain.Core.Logic.Models.Tokens;

namespace Auth.Domain.Interface.Logic.External.Social
{
    public interface IAuthFacebook
    {
        Task<SocialData> GetTokenInfoAsync(string token);
    }
}
