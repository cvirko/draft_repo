namespace Auth.Domain.Core.Common.Tools.Configurations
{
    public class MailOptions : Options
    {
        public string Host { get; set; }
        public short Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ApplicationUrl { get; set; }
        public string BackgroundImgURL { get; set; }
        public string MessageHtmlURL { get; set; }
        public string TeamName { get; set; }
        public float DelayBetweenMessagesInMinutes { get; set; }
    }
}
