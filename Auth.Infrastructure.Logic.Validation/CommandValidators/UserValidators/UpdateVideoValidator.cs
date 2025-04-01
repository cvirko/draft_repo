using Auth.Domain.Core.Logic.Commands.User;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class UpdateVideoValidator(IUnitOfWorkValidationRule rule) 
        : Validator<UpdateVideoCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateVideoCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());
            RuleFor().File().IsVideo(command.File, command.ContentType);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
