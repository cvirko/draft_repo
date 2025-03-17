namespace Auth.Client.ConsoleApp.Interfaces
{
    public interface IChatApiService
    {
        Task JoinAsync(string name, string token);
        Task LeaveAsync(string name, string token);
        Task SendAsync(string name, string token);
    }
}
