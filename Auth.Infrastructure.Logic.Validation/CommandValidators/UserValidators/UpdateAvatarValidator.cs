using Auth.Domain.Core.Logic.Commands.User;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class UpdateAvatarValidator(IUnitOfWorkValidationRule rule) 
        : Validator<UpdateAvatarCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateAvatarCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());
            RuleFor().File().IsImage(command.Avatar, command.ContentType);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
