using Library.CommandMediator.Models;

namespace Library.CommandMediator.Interfaces
{
    public interface ICommandDispatcher
    {
        Task<TResponse> ProcessAsync<TCommand, TResponse>(TCommand command)
            where TCommand : CommandBase<TResponse>;
        public Task<TResponse> ProcessAsync<TResponse>(CommandBase<TResponse> command);
        public Task ProcessAsync<TCommand>(TCommand command)
            where TCommand : CommandBase;
    }
}
