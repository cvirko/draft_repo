namespace Auth.Client.ConsoleApp.Interfaces
{
    public interface IServerClientService : IDisposable
    {
        Task<T> PostAsync<T, V>(V body, string requesUri, string token = null);
        Task PostAsync<V>(V body, string requesUri, string token = null);
        Task PostAsync(string requesUri, string token = null);
        Task<string> GetAsync(string requesUri, string token = null);
        Task<T> GetAsync<T>(string requesUri, string token = null);
        Task<T> GetAsync<T>(string requesUri, string token, params object[] routes);
    }
}
