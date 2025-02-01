namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class TokenOptions : Options
    {
        public string TokenSecret { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string AppUrl { get; set; }

        public float RefreshTokenExpiresTimeInMinutes { get; set; }
        public float AccessTokenExpiresTimeInMinutes { get; set; }
        public float ConfirmationTokenExpiresTimeInMinutes { get; set; }
    }
}
