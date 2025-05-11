using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Logic.Commands.Account;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class UpdateRefreshTokenHandler(IOptionsSnapshot<FailedAccessOptions> option, IUnitOfWork uow)
        : Handler<UpdateRefreshTokenCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly FailedAccessOptions _options = option.Value;
        public override async Task HandleAsync(UpdateRefreshTokenCommand command)
        {
            var userToken = await GetOrAddAsync(command.UserLoginInfo, command.UserId);

            userToken.Token = command.Token;
            userToken.DateAt = DateTimeExtension.Get();
            userToken.Attempts = _options.FailedAccessAttemptsMaxCount;
            userToken.IsConfirmed = false;
            await _uow.SaveAsync();
        }
        private async Task<UserToken> GetOrAddAsync(string userLoginInfo, Guid userId)
        {
            var userToken = await _uow.Users().GetUserTokenAsync(userLoginInfo, userId,  TokenType.Refresh);
            if (userToken != null)
                return userToken;
            userToken = new()
            {
                UserInfo = userLoginInfo,
                TokenType = TokenType.Refresh,
                UserId = userId,
            };
            await _uow.AddAsync(userToken);
            return userToken;
        }
    }
}
