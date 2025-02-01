using Auth.Domain.Core.Logic.Commands;

namespace Auth.Domain.Interface.Logic.Write.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        Task HandleAsync(TCommand command);
    }
}
