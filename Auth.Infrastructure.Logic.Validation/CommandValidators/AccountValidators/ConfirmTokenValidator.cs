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
            if (command.TokenType == TokenType.ConfirmMail ||
                command.TokenType == TokenType.Reset)
                RuleFor(p => p.Token).Token().IsOnlyNumbers(command.Token);
            RuleFor().User().IsLengthFormatValid(command.UserId.ToString());
            RuleFor().User().IsLengthFormatValid(command.TokenLoginId.ToString());

            if (IsInvalid()) return GetErrors();
            var token = await _uow.Users().GetUserTokenAsync(command.TokenLoginId, command.UserId, command.TokenType);
            var exp = command.TokenType == TokenType.Refresh 
                ? _tokenService.RefreshTokenExpiresTimeInMinutes 
                : _tokenService.ConfirmationTokenExpiresTimeInMinutes;
            RuleFor(p => p.Token).Token().IsHaveAttempts(token, exp);

            return GetErrors();
        }
    }
}
