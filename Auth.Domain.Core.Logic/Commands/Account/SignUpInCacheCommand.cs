namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class SignUpInCacheCommand : Command
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
