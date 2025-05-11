namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class SendResetPassMessageCommand : Command
    {
        public SendResetPassMessageCommand(){ }
        public SendResetPassMessageCommand(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
        [JsonIgnore]
        public string UserInfo { get; set; }
    }
}
