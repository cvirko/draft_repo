namespace Auth.Client.ConsoleApp.Interfaces
{
    public interface IUnitOfWorkServerApi : IDisposable
    {
        public IAccountService Account();
        public IChatApiService Chat();
    }
}
