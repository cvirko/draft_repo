using Auth.Domain.Core.Logic.Commands.User;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class UpdateVideoValidator(IValidationRuleService validate) 
        : Validator<UpdateVideoCommand>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateVideoCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor().File().IsVideo(command.File, command.ContentType);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
