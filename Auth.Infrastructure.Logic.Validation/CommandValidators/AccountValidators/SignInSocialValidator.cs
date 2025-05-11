using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class SignInSocialValidator(IValidationRuleService validate) 
        : Validator<SignInSocialCommand>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(SignInSocialCommand command)
        {
            if (string.IsNullOrEmpty(command.Info?.Email))
                AddError(ErrorStatus.AccessDenied);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
