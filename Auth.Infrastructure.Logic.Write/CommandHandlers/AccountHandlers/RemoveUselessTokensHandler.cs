using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Account;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class RemoveUselessTokensHandler(IOptionsSnapshot<TokenOptions> option, IUnitOfWork uow)
        : Handler<RemoveUselessTokensCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly TokenOptions _options = option.Value;
        public override async Task HandleAsync(RemoveUselessTokensCommand command)
        {
            var date = _options.RefreshTokenExpiresTimeInMinutes + 10;
            await _uow.RemoveTokensBeforeAsync(DateTimeExtension.WithMinutes(-date), command.Token);
        }
    }
}
