using Auth.Domain.Core.Logic.Commands.User;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.UserValidators
{
    internal class UpdateUserNameValidator(IValidationRuleService validate, IUnitOfWorkRead uow) 
        : Validator<UpdateUserNameCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(UpdateUserNameCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor(p => p.UserName).Name().IsLengthFormatValid(command.UserName);
            if (IsInvalid) return GetErrors();
            var user = await _uow.Users().GetUserAsync(command.UserId);
            if (RuleFor().User().IsExist(user))
                RuleFor().User().IsNotEqual(user.UserName, command.UserName);
            return GetErrors();
        }
    }
}
