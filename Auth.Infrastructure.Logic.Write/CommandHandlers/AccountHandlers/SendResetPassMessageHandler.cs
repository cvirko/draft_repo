using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.UOW;
using Auth.Domain.Interface.Logic.Notification.Mail;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class SendResetPassMessageHandler(IMailService mail, ITokenBuilder token, 
        ICommandDispatcher dispatcher, IUnitOfWorkRead uow)
        : ICommandHandler<SendResetPassMessageCommand>
    {
        private readonly IMailService _mail = mail;
        private readonly ITokenBuilder _token = token;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        private readonly IUnitOfWorkRead _uow = uow;
        public async Task HandleAsync(SendResetPassMessageCommand command)
        {
            var tokenData = _token.CreateNumericToken();
            var login = await _uow.Users().GetLoginByEmailAsync(command.Email);
            await _dispatcher.ProcessAsync(
                new UpdateTokenCommand(login.UserId, tokenData.Token, TokenType.Reset, command.UserInfo));
            

            _ = _mail.SendAsync(new()
            {
                EmailTo = command.Email,
                MessageType = EmailMessageType.Reset,
                UserName = login.User.UserName,
                Token = tokenData.Token,
                ExpiresInMinutes = (tokenData.Expires - DateTimeExtension.Get()).TotalMinutes,
            }).ConfigureAwait(false);
        }
    }
}
