using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Logic.External.Auth;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class ConfirmTokenValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow,
        ITokenService token) 
        : Validator<ConfirmTokenCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly ITokenService _tokenService = token;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(ConfirmTokenCommand command)
        {
            RuleFor(p => p.Token).Token().IsLengthFormatValid(command.Token);
            RuleFor(p => p.Token).Token().IsOnlyNumbers(command.Token);
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());

            if (IsInvalid()) return GetErrors();
            var token = await _uow.Users().GetUserTokenAsync(command.UserId, command.TokenType);
            RuleFor(p => p.Token).Token().IsHaveAttempts(token, _tokenService.ConfirmationTokenExpiresTimeInMinutes);

            return GetErrors();
        }
    }
}
