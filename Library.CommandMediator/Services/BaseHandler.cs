using Library.CommandMediator.Interfaces;
using Library.CommandMediator.Models;

namespace Library.CommandMediator.Services
{
    public abstract class BaseHandler<TCommand> :
        ICommandHandler<TCommand>
        where TCommand : CommandBase
    {
        public abstract Task HandleAsync(TCommand command);
    }

    public abstract class BaseHandler<TCommand, TResponse> :
        ICommandHandler<TCommand, TResponse>
        where TCommand : CommandBase<TResponse>
    {
        public async Task<TResponse> HandleAsync(CommandBase<TResponse> command)
            => await HandleAsync((TCommand)command);
        public abstract Task<TResponse> HandleAsync(TCommand command);
    }
}
