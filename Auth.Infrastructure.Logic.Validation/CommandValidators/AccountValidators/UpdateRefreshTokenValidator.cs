using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class UpdateRefreshTokenValidator(IUnitOfWorkValidationRule rule) 
        : Validator<UpdateRefreshTokenCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateRefreshTokenCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());
            RuleFor().Token().IsLengthFormatValid(command.Token);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
