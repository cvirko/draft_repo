using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.External.Social;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auth.Infrastructure.Logic.Read.ModelBuilders.AccountBuilders
{
    internal class AccountBuilder(IUnitOfWorkRead uow, IUserMapper mapper, IAuthGoogle google,
        IAuthFacebook facebook) : IAccountBuilder
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly IUserMapper _mapper = mapper;
        private readonly IAuthGoogle _google = google;
        private readonly IAuthFacebook _facebook = facebook;

        public async Task<SocialData> GetBySocialAsync(string token, SocialType social)
        {
            switch (social)
            {
                case SocialType.Google:
                    return await _google.GetTokenInfoAsync(token);
                case SocialType.Facebook:
                    return await _facebook.GetTokenInfoAsync(token);
                default:
                    throw new NotImplementedException($"{social} no implemented");
            }
        }
        public async Task<LoginDTO> GetLoginAsync(string email, HttpRequest request)
        {
            var login = await _uow.Users().GetLoginByEmailAsync(email);
            return _mapper.Map(login, request);
        }
        public async Task<LoginDTO> GetLoginAsync(ClaimsPrincipal user, HttpRequest request)
        {
            var login = await _uow.Users().GetLoginByUserIdAsync(user.GetUserId(),user.GetLoginId());
            var result = _mapper.Map(login, request);
            result.LoginDate = user.GetDate();
            return result;
        }
    }
}
