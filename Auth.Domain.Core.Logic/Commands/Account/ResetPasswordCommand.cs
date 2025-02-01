namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class ResetPasswordCommand : Command
    {
        public string Password { get; set; }
    }
}
