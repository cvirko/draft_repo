using Auth.Client.ConsoleApp.Interfaces.Api;
using Auth.Client.ConsoleApp.Services.Api.Controllers;

namespace Auth.Client.ConsoleApp.Services.Api
{
    internal class UnitOfWorkServerApi(string uri) : IUnitOfWorkApi
    {
        private readonly IApiClientService _context = new ApiClientService(uri);

        private IAccountService _account;
        private IChatApiService _chat;
        public IAccountService Account()
        {
            if (_account == null) _account = new AccountService(_context);
            return _account;
        }
        public IChatApiService Chat()
        {
            if (_chat == null) _chat = new ChatApiService(_context);
            return _chat;
        }
        public void Dispose()
        {
            _account?.Dispose();
            _context.Dispose();
        }
    }
}
