using Auth.Domain.Core.Logic.Commands;
using Auth.Domain.Interface.Logic.Read.Validators;

namespace Auth.Domain.Interface.Logic.Write.Commands
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> Resolve<T>() where T : Command;
        IValidator<T> ValidatorResolve<T>() where T : Command;
    }
}
