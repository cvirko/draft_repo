namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class AuthOptions : Options
    {
        public SocialOtions Google { get; set; }
        public SocialOtions Twitter { get; set; }
        public SocialOtions Facebook { get; set; }
    }
    public class SocialOtions
    {
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
    }
}
