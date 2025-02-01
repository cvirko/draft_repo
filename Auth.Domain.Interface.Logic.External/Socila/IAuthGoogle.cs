using Auth.Domain.Core.Logic.Models.Tokens;

namespace Auth.Domain.Interface.Logic.External.Socila
{
    public interface IAuthGoogle
    {
        Task<SocialData> GetTokenInfoAsync(string token);
    }
}
