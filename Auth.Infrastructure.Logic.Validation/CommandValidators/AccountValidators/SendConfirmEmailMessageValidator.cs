using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.Cache;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class SendConfirmEmailMessageValidator(IValidationRuleService validate, IUnitOfWorkRead uow,
        IOptionsSnapshot<MailOptions> options, ICacheRepository cache) 
        : Validator<SendConfirmEmailMessageCommand>(validate)
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly ICacheRepository _cache = cache;
        private readonly MailOptions _mailOptions = options.Value;
        public override async Task<IEnumerable<ValidationError>> ValidateAsync(SendConfirmEmailMessageCommand command)
        {
            RuleFor(p => p.Email).Email().IsLengthFormatValid(command.Email);
            RuleFor(p => p.UserName).Name().IsLengthFormatValid(command.UserName);
            RuleFor().User().IsLengthFormatValid(command.UserId);
            if (IsInvalid) return GetErrors();

            var token = await _uow.Users().GetUserTokenAsync(command.UserInfo, command.UserId, TokenType.ConfirmMail);
            if (!RuleFor(p => p.Email).Email()
                .IsDelayGone(command.Email, token?.DateAt, _mailOptions.DelayBetweenMessagesInMinutes))
                return GetErrors();

            if (!await RuleFor(p => p.Email).Email().IsNotOccupiedAsync(command.Email, _uow.Users().IsExistEmailAsync))
                return GetErrors();
            await RuleFor(p => p.Email).Email().IsExistAsync(_cache.GetSignUpEmailKey(command.Email),
                    _cache.IsExistDataAsync);

            return GetErrors();
        }
    }
}
