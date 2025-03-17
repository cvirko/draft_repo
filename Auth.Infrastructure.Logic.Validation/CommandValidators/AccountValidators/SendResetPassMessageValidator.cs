using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Account;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class SendResetPassMessageValidator(IUnitOfWorkValidationRule rule, IUnitOfWorkRead uow,
        IOptionsSnapshot<MailOptions> options) 
        : Validator<SendResetPassMessageCommand>(rule)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly MailOptions _mailOptions = options.Value;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(SendResetPassMessageCommand command)
        {
            RuleFor(p => p.Email).Email().IsLengthFormatValid(command.Email);

            if (IsInvalid()) return GetErrors();
            var login = await _uow.Users().GetLoginByEmailAsync(command.Email);
            if (!RuleFor(p => p.Email).Email().IsExist(login))
                return GetErrors();

            var token = await _uow.Users().GetUserTokenAsync(login.UserId, TokenType.Reset);
            RuleFor(p => p.Email).Email().IsDelayGone(command.Email, token?.CreationDate, _mailOptions.DelayBetweenMessagesInMinutes);

            return GetErrors();
        }
    }
}
