namespace Auth.Client.ConsoleApp.Interfaces.Api
{
    internal interface IUnitOfWorkApi : IDisposable
    {
        public IAccountService Account();
        public IChatApiService Chat();
    }
}
