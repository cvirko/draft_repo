namespace Auth.Client.ConsoleApp.Interfaces
{
    public interface IAccountService: IDisposable
    {
        Task TryLoginAsync();
        string Token { get; }
    }
}
