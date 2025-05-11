namespace Auth.Api.Controllers.Write
{
    public abstract class CommandController(ICommandDispatcher command) : ControllerBase
    {
        protected readonly ICommandDispatcher _command = command;
        protected async Task ProcessAsync<T>(T command) where T : Command
        {
            command.UserId = User.GetUserId();
            command.LoginId = User.GetLoginId();
            await _command.ProcessAsync(command);
        }

        protected async Task<TResponse> ProcessAsync<TResponse>(Command<TResponse> command)
        {
            command.UserId = User.GetUserId();
            command.LoginId = User.GetLoginId();
            return await _command.ProcessAsync(command);
        }

        protected async Task<TResponse> ProcessAsync<TCommand, TResponse>(TCommand command)
            where TCommand : Command<TResponse>
        {
            command.UserId = User.GetUserId();
            command.LoginId = User.GetLoginId();
            return await _command.ProcessAsync(command);
        }
    }
}
