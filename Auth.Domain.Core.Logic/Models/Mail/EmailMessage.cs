namespace Auth.Domain.Core.Logic.Models.Mail
{
    public class EmailMessage
    {
        public string EmailTo { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public double ExpiresInMinutes { get; set; }
        public EmailMessageType MessageType { get; set; }
    }
}
