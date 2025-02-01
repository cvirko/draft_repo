using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class UpdateTokenValidator(IUnitOfWorkValidationRule rule) 
        : Validator<UpdateTokenCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(UpdateTokenCommand command)
        {
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());
            RuleFor().User().IsLengthFormatValid(command.TokenLoginId.ToString());
            RuleFor().Token().IsLengthFormatValid(command.Token);
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}
