using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Account;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class UpdateTokenHandler(IOptionsSnapshot<FailedAccessOptions> option, IUnitOfWork uow)
        : ICommandHandler<UpdateTokenCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly FailedAccessOptions _options = option.Value;
        public async Task HandleAsync(UpdateTokenCommand command)
        {
            var userToken = await GetOrAddAsync(command.TokenLoginId, command.UserId, command.Type);

            userToken.Token = command.Token;
            userToken.CreationDate = DateTimeExtension.Get();
            userToken.Attempts = _options.FailedAccessAttemptsMaxCount;
            userToken.IsConfirmed = false;
            await _uow.SaveAsync();
        }
        private async Task<UserToken> GetOrAddAsync(Guid tokenLoginId, Guid userId, TokenType type)
        {
            var userToken = await _uow.Users().GetUserTokenAsync(tokenLoginId, userId, type);
            if (userToken != null)
                return userToken;
            userToken = new()
            {
                UserTokenId = tokenLoginId,
                TokenType = type,
                UserId = userId,
            };
            await _uow.AddAsync(userToken);
            return userToken;
        }
    }
}
