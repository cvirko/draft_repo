using Library.CommandMediator.Models;

namespace Auth.Domain.Core.Logic.Commands
{
    public record ValidationError : BaseValidationError<ErrorStatus>
    {
        public ValidationError()
        {
        }
        public ValidationError(
            string field,
            string title = default,
            ErrorStatus type = default,
            string description = default) : base(field, title, type, description)
        { }
    }
}
