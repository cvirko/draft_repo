using Auth.Domain.Core.Logic.Commands;

namespace Auth.Domain.Interface.Logic.Write.Commands
{
    public interface ICommandDispatcher
    {
        Task ProcessAsync(Command command);
    }
}
