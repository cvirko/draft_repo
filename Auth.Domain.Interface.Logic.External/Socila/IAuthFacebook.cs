using Auth.Domain.Core.Logic.Models.Tokens;

namespace Auth.Domain.Interface.Logic.External.Socila
{
    public interface IAuthFacebook
    {
        Task<SocialData> GetTokenInfoAsync(string token);
    }
}
