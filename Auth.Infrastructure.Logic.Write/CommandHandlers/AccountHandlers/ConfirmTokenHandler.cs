using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class ConfirmTokenHandler(IUnitOfWork uow, IValidationRuleService validate)
        : Handler<ConfirmTokenCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IValidationRuleService _validate = validate;
        public override async Task HandleAsync(ConfirmTokenCommand command)
        {
            var userToken = await _uow.Users()
                .GetUserTokenAsync("",command.UserId, command.TokenType);
            _validate.SetFieldName(nameof(command.Token));
            if (_validate.Token().IsMatch(command.Token, userToken))
            {
                userToken.IsConfirmed = true;
            }
            else
            {
                userToken.Attempts--;
            }

            await _uow.SaveAsync();

            if (!_validate.IsValid)
                _validate.Throw(command.GetType());
        }
    }
}
