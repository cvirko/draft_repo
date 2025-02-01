namespace Auth.Domain.Core.Logic.Commands.User
{
    public class UpdatePasswordCommand : Command
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
