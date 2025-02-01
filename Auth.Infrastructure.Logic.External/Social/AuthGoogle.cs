using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.External.Socila;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.External.Social
{
    internal class AuthGoogle(IOptionsSnapshot<AuthOptions> options) : IAuthGoogle
    {
        private readonly AuthOptions _options = options.Value;

        public async Task<SocialData> GetTokenInfoAsync(string token)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_options.Google.ClientId]
            });
            if (!payload.EmailVerified) return null;
            return new SocialData
            {
                Email = payload.Email,
                Name = payload.GivenName,
                Picture = payload.Picture,
            };
        }
    }
}
