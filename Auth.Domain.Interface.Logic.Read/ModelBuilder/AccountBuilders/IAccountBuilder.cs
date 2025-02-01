using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using System.Security.Claims;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.AccountBuilders
{
    public interface IAccountBuilder: IBuilder
    {
        Task<SocialData> GetBySocialAsync(string token, SocialType social);
        Task<LoginDTO> GetLoginAsync(string email);
        Task<LoginDTO> GetLoginAsync(ClaimsPrincipal user);
    }
}
