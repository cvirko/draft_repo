using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.AccountBuilders
{
    public interface IAccountBuilder: IBuilder
    {
        Task<SocialData> GetBySocialAsync(string token, SocialType social);
        Task<LoginDTO> GetLoginAsync(string email, HttpRequest request);
        Task<LoginDTO> GetLoginAsync(ClaimsPrincipal user, HttpRequest request);
    }
}
