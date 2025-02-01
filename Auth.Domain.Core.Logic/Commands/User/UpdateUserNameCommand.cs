namespace Auth.Domain.Core.Logic.Commands.User
{
    public class UpdateUserNameCommand : Command
    {
        public string UserName { get; set; }
    }
}
