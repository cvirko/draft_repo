using Auth.Client.ConsoleApp.Interfaces;
namespace Auth.Client.ConsoleApp.Services.Api
{
    internal class UnitOfWorkServerApi(string uri) : IUnitOfWorkServerApi
    {
        private readonly IServerClientService _context = new ServerClientService(uri);

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
