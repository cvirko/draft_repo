using Auth.Domain.Core.Data.DBEntity.Account;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Data.Read.Cache;
namespace Auth.Infrastructure.Logic.Write.CommandHandlers.AccountHandlers
{
    internal class CreateAccountFromCacheHandler(IUnitOfWork uow,
        ICacheRepository cache) : Handler<CreateAccountFromCacheCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly ICacheRepository _cache = cache;
        public override async Task HandleAsync(CreateAccountFromCacheCommand command)
        {
            UserLogin login;
            if (await _uow.Users().IsExistUserAsync(command.UserId))
            {
                login = await _cache.GetDataAsync<UserLogin>(_cache.GetSignUpEmailKey(command.Email));
            }
            else
            {
                var user = await _cache.GetDataAsync<User>(_cache.GetSignUpEmailKey(command.Email));
                login = user.Logins.First();
                user.Logins = null;
                
                await _uow.AddAsync(user);
                await _uow.AddAsync(new UserWallet
                {
                    Balance = 0,
                    UserId = user.UserId
                });
            }
            login.LoginId = default;
            await _uow.AddAsync(login);
            await _uow.SaveAsync();
        }
    }
}
