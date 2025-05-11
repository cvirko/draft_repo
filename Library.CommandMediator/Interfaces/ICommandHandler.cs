using Library.CommandMediator.Models;

namespace Library.CommandMediator.Interfaces
{
    public interface ICommandHandler<in TCommand> 
        where TCommand : CommandBase
    {
        public Task HandleAsync(TCommand command);
    }
    public interface ICommandHandler<in TCommand, TResponse>
        : ICommandHandlerWithResponse<TResponse>
        where TCommand : CommandBase<TResponse>
    {
        public Task<TResponse> HandleAsync(TCommand command);
    }
    public interface ICommandHandlerWithResponse<TResponse>
    {
        public Task<TResponse> HandleAsync(CommandBase<TResponse> command);
    }
}
