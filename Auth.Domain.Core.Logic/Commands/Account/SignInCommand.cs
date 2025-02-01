namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class SignInCommand : Command
    {
        public string Login { get; set; }
        public string Password { get; set; }

    }
}
