namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class SendConfirmEmailMessageCommand : Command
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string UserInfo { get; set; }
    }
}
