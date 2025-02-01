using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class ResetPasswordValidator(IUnitOfWorkValidationRule rule) 
        : Validator<ResetPasswordCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(ResetPasswordCommand command)
        {
            RuleFor(p => p.Password).Password().IsLengthFormatValid(command.Password);
            RuleFor(p => p.UserId).User().IsLengthFormatValid(command.UserId.ToString());

            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
