using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class UpdateRefreshTokenValidator(IValidationRuleService validate) 
        : Validator<UpdateRefreshTokenCommand>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateRefreshTokenCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor().Token().IsLengthFormatValid(command.Token);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
