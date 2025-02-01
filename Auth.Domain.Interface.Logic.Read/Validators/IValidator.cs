using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Core.Logic.Commands;

namespace Auth.Domain.Interface.Logic.Read.Validators
{
    public interface IValidator<in TCommand> where TCommand : Command
    {
        public Task<IEnumerable<ValidationError>> ValidateAsync(TCommand command);
        public bool IsValid();
    }
}
