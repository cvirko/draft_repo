using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class ConfirmTokenHandler(IUnitOfWork uow, IUnitOfWorkValidationRule uowVRule)
        : ICommandHandler<ConfirmTokenCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IUnitOfWorkValidationRule _uowValidationRule = uowVRule;
        public async Task HandleAsync(ConfirmTokenCommand command)
        {
            var userToken = await _uow.Users()
                .GetUserTokenAsync(command.UserId, command.TokenType);
            _uowValidationRule.SetFieldName(nameof(command.Token));
            if (_uowValidationRule.Token().IsMatch(command.Token, userToken))
            {
                userToken.IsConfirmed = true;
            }
            else
            {
                userToken.Attempts--;
            }

            await _uow.SaveAsync();

            if (!_uowValidationRule.IsValid())
                throw new ForbiddenException(_uowValidationRule.GetErrors());
        }
    }
}
