using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Domain.Interface.Logic.External.Auth;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class SignUpInCacheHandler(IPasswordHasherService passwordHasher,
        IOptionsSnapshot<FailedAccessOptions> option, ICacheRepository cache,
        ITokenService token) : ICommandHandler<SignUpInCacheCommand>
    {
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        private readonly FailedAccessOptions _option = option.Value;
        private readonly ICacheRepository _cache = cache;
        private readonly ITokenService _token = token;
        public async Task HandleAsync(SignUpInCacheCommand command)
        {
            var user = new User(Guid.NewGuid(), RoleType.User, UserStatus.Active);
            user.UserName = command.UserName;
            user.Logins =[ new UserLogin
            {
                LoginId = 1,
                Email = command.Email,
                UserId = user.UserId,
                PasswordHash = _passwordHasher.HashPassword(user.UserId, command.Password),
                Attempts = _option.FailedAccessAttemptsMaxCount,
                CreationDate = DateTimeExtension.Get()
            }];
            command.UserId = user.UserId;
            await _cache.SetDataAsync(_cache.GetSignUpEmailKey(command.Email), user, 
                TimeSpan.FromMinutes(_token.ConfirmationTokenExpiresTimeInMinutes + 1));
        }
    }
}
