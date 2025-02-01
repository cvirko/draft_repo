namespace Auth.Api.Controllers.Write
{
    public abstract class CommandController(ICommandDispatcher command) : ControllerBase
    {
        protected readonly ICommandDispatcher _command = command;
        protected async Task ProcessAsync(Command command)
        {
            command.UserId = User.GetUserId();
            command.LoginId = User.GetLoginId();
            await _command.ProcessAsync(command);
        }
    }
}
