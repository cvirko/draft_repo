namespace Auth.Client.ConsoleApp.Interfaces.Api
{
    public interface IAccountService : IDisposable
    {
        Task TryLoginAsync();
        string Token { get; }
    }
}
