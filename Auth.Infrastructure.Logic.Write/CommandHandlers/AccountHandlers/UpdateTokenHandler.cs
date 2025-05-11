using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Logic.Commands.Account;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class UpdateTokenHandler(IOptionsSnapshot<FailedAccessOptions> option, IUnitOfWork uow)
        : Handler<UpdateTokenCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly FailedAccessOptions _options = option.Value;
        public override async Task HandleAsync(UpdateTokenCommand command)
        {
            var userToken = await GetOrAddAsync(command.UserId, command.Type,command.UserLoginInfo);

            userToken.Token = command.Token;
            userToken.DateAt = DateTimeExtension.Get();
            userToken.Attempts = _options.FailedAccessAttemptsMaxCount;
            userToken.IsConfirmed = false;
            await _uow.SaveAsync();
        }
        private async Task<UserToken> GetOrAddAsync(Guid userId, TokenType type, string info)
        {
            var userToken = await _uow.Users().GetUserTokenAsync(info, userId, type);
            if (userToken != null)
            {
                return userToken;
            }
                
            userToken = new()
            {
                UserInfo = info,
                TokenType = type,
                UserId = userId,
            };
            await _uow.AddAsync(userToken);
            return userToken;
        }
    }
}
