using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Domain.Interface.Logic.External.Auth;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.UserHandlers
{
    internal class AddLoginInCacheHandler(IPasswordHasherService passwordHasher,
        ICacheRepository cache, IOptionsSnapshot<FailedAccessOptions> option, ITokenService token)
        : ICommandHandler<AddLoginInCacheCommand>
    {
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        private readonly ICacheRepository _cache = cache;
        private readonly FailedAccessOptions _option = option.Value;
        private readonly ITokenService _token = token;

        public async Task HandleAsync(AddLoginInCacheCommand command)
        {
            var login = new UserLogin
            {
                Email = command.Email,
                UserId = command.UserId,
                PasswordHash = _passwordHasher.HashPassword(command.UserId, command.Password),
                Attempts = _option.FailedAccessAttemptsMaxCount,
                CreationDate = DateTimeExtension.Get()
            };

            await _cache.SetDataAsync(_cache.GetSignUpEmailKey(command.Email), login,
                TimeSpan.FromMinutes(_token.ConfirmationTokenExpiresTimeInMinutes + 1));
        }
    }
}
