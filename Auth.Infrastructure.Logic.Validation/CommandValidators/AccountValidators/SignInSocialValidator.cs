using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class SignInSocialValidator(IUnitOfWorkValidationRule rule) 
        : Validator<SignInSocialCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(SignInSocialCommand command)
        {
            if (string.IsNullOrEmpty(command.Info?.Email))
                AddError(ErrorStatus.AccessDenied);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
