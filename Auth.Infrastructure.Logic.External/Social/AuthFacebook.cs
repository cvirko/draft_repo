using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.External.Social;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;

namespace Auth.Infrastructure.Logic.External.Social
{
    internal class AuthFacebook(IOptionsSnapshot<AuthOptions> options, 
        IHttpClientFactory httpClientFactory) : IAuthFacebook
    {
        private readonly AuthOptions _options = options.Value;
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private const string TOKEN_VALIDATION_URL = "/debug_token?input_token={0}&access_token={1}|{2}";
        private const string USER_INFO_URL = "/me?fields=id,name,email,picture&access_token={0}";
        public async Task<SocialData> GetTokenInfoAsync(string token)
        {
            if (!await IsValidAsync(token))
                return null;

            var formatedUrl = string.Format(USER_INFO_URL, token);
            var userInfoResponse = await _httpClient.GetAsync(formatedUrl);
            if (!userInfoResponse.IsSuccessStatusCode) return null;

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonConvert.DeserializeObject<FacebookUserInfo>(userInfoContent);
            return new SocialData
            {
                Email = userInfo.Email,
                Name = userInfo.Name,
                Picture = userInfo.Picture.Data.Url,
            };
        }
        private async Task<bool> IsValidAsync(string token)
        {
            var formatedUrl = string.Format(TOKEN_VALIDATION_URL, token, 
                _options.Facebook.ClientId, _options.Facebook.ClientSecret);

            var validationResponse = await _httpClient.GetAsync(formatedUrl);
            if (!validationResponse.IsSuccessStatusCode) return false;

            var validationContent = await validationResponse.Content.ReadAsStringAsync();
            var tokenValidation = JsonConvert.DeserializeObject<FacebookTokenValidation>(validationContent);

            return tokenValidation.Data?.IsValid ?? false;
        }
        private class FacebookUserInfo
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("picture")]
            public FacebookPicture Picture { get; set; }
        }
        private class FacebookTokenValidation
        {
            [JsonProperty("data")]
            public FacebookTokenValidationData Data { get; set; }
        }

        private class FacebookTokenValidationData
        {
            [JsonProperty("is_valid")]
            public bool IsValid { get; set; }
            [JsonProperty("app_id")]
            public string AppId { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("application")]
            public string Application { get; set; }
            [JsonProperty("user_id")]
            public string UserId { get; set; }
            [JsonProperty("expires_at")]
            public long ExpiresAt { get; set; }
            [JsonProperty("scopes")]
            public string[] Scopes { get; set; }
        }
        private class FacebookPicture
        {
            [JsonProperty("data")]
            public FacebookPictureData Data { get; set; }
        }

        private class FacebookPictureData
        {
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("is_silhouette")]
            public bool IsSilhouette { get; set; }
        }
    }
}
