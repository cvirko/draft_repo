namespace Auth.Domain.Core.Logic.Commands.Account
{
    public class CreateAccountFromCacheCommand : Command
    {
        public CreateAccountFromCacheCommand()
        {
            
        }
        public CreateAccountFromCacheCommand(string email)
        {
            Email = email;
        }
        public string Email { get; set; }
    }
}
