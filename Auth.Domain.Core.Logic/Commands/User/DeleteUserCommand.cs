namespace Auth.Domain.Core.Logic.Commands.User
{
    public class DeleteUserCommand : Command
    {
        public string Password { get; set; }
    }
}
