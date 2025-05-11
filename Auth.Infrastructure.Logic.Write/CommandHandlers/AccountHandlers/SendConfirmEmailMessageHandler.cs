using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Logic.Notification.Mail;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class SendConfirmEmailMessageHandler(IMailService mail, ITokenBuilder token,
        ICommandDispatcher dispatcher)
        : Handler<SendConfirmEmailMessageCommand>
    {
        private readonly IMailService _mail = mail;
        private readonly ITokenBuilder _token = token;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        public override async Task HandleAsync(SendConfirmEmailMessageCommand command)
        {
            var tokenData = _token.CreateNumericToken();
            await _dispatcher.ProcessAsync(
                new UpdateTokenCommand(command.UserId, tokenData.Token,TokenType.ConfirmMail, command.UserInfo));

            _ = _mail.SendAsync(new()
            {
                EmailTo = command.Email,
                MessageType = EmailMessageType.Verification,
                UserName = command.UserName,
                Token = tokenData.Token,
                ExpiresInMinutes = (tokenData.Expires - DateTimeExtension.Get()).TotalMinutes,
            }).ConfigureAwait(false);
        }
    }
}
