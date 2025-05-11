using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class UpdateTokenValidator(IValidationRuleService validate) 
        : Validator<UpdateTokenCommand>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateTokenCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId);
            RuleFor().Token().IsLengthFormatValid(command.Token);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
