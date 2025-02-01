namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class SendResetPassMessageCommand(string email) : Command
    {
        public string Email { get; set; } = email;
    }
}
