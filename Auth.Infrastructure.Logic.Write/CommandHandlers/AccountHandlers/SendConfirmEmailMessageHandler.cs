using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Logic.External.Mail;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class SendConfirmEmailMessageHandler(IMailService mail, ITokenBuilder token,
        ICommandDispatcher dispatcher)
        : ICommandHandler<SendConfirmEmailMessageCommand>
    {
        private readonly IMailService _mail = mail;
        private readonly ITokenBuilder _token = token;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        public async Task HandleAsync(SendConfirmEmailMessageCommand command)
        {
            var tokenData = _token.CreateNumericToken();
            await _dispatcher.ProcessAsync(
                new UpdateTokenCommand(command.TokenLoginId, command.UserId, tokenData.Token,
                 TokenType.ConfirmMail));

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
