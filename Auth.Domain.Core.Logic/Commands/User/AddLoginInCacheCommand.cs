namespace Auth.Domain.Core.Logic.Commands.User
{
    public class AddLoginInCacheCommand : Command
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
