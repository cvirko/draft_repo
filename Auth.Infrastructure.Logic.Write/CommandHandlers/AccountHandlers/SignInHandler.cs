using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Logic.External.Auth;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class SignInHandler(IUnitOfWork uow, IUnitOfWorkValidationRule uowVRule,
        IOptionsSnapshot<FailedAccessOptions> option, IPasswordHasherService passwordHasher)
        : ICommandHandler<SignInCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IUnitOfWorkValidationRule _uowValidationRule = uowVRule;
        private readonly FailedAccessOptions _option = option.Value;
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        public async Task HandleAsync(SignInCommand command)
        {
            var login = await _uow.Users().GetLoginByEmailAsync(command.Login);

            if (_uowValidationRule.Password().IsMatch(login,command.Password, _passwordHasher.VerifyHashedPassword))
            {
                login.Attempts = _option.FailedAccessAttemptsMaxCount;
                login.LastLoginDate = DateTimeExtension.Get();
                login.User.LastLoginDate = DateTimeExtension.Get();
            }
            else
            {
                login.Attempts--;
                if (login.Attempts == 0)
                    login.BanExpireDate = DateTimeExtension.WithMinutes(_option.TimeLockInMinutes);
            }

            await _uow.SaveAsync();

            if (!_uowValidationRule.IsValid())
                throw new ForbiddenException(_uowValidationRule.GetErrors());
        }
    }
}
