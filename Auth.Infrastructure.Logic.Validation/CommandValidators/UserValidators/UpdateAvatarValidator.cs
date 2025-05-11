using Auth.Domain.Core.Logic.Commands.User;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class UpdateAvatarValidator(IValidationRuleService validate) 
        : Validator<UpdateAvatarCommand>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateAvatarCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor().File().IsImage(command.Avatar, command.ContentType);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
