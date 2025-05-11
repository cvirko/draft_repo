using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class SignInSocialHandler(IUnitOfWork uow,
        IOptionsSnapshot<FailedAccessOptions> option, 
        ICommandDispatcher dispatcher, IFileBuilder file)
        : Handler<SignInSocialCommand>
    {
        private readonly IFileBuilder _file = file;
        private readonly IUnitOfWork _uow = uow;
        private readonly FailedAccessOptions _option = option.Value;
        private readonly ICommandDispatcher _dispatcher = dispatcher;
        public override async Task HandleAsync(SignInSocialCommand command)
        {
            var login = await _uow.Users().GetLoginByEmailAsync(command.Info.Email);

            if (login is null)
                login = await CreateAccountAsync(command.Info);

            login.Attempts = _option.FailedAccessAttemptsMaxCount;
            login.LastLoginDate = DateTimeExtension.Get();
            login.User.LastLoginDate = DateTimeExtension.Get();

            await _uow.SaveAsync();
        }
        private async Task<UserLogin> CreateAccountAsync(SocialData info)
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, RoleType.User, UserStatus.Active);
            user.UserName = info.Name;
            await _uow.AddAsync(user);
            var login = new UserLogin
            {
                Email = info.Email,
                UserId = userId,
                User = user,
                Attempts = _option.FailedAccessAttemptsMaxCount,
                DateAt = DateTimeExtension.Get()
            };
            await _uow.AddAsync(login);
            await AddAvatarAsync(info.Picture, userId);
            return login;
        }
        private async Task AddAvatarAsync(string uri, Guid userId)
        {
            var avatar = await _file.ReadFileByUriAsync(uri);
            if (avatar == null) return;
            try
            {
                var command = new UpdateAvatarCommand(avatar.Stream, avatar.ContentType);
                command.UserId = userId;
                await _dispatcher.ProcessAsync(command);
                
            }
            catch
            {
                return;
            }
            finally
            {
                avatar.Stream?.Dispose();
            }
        }
    }
}
