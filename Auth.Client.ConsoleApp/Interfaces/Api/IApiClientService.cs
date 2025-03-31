namespace Auth.Client.ConsoleApp.Interfaces.Api
{
    internal interface IApiClientService : IDisposable
    {
        Task<T> PostAsync<T, V>(V body, string requesUri, string token = null);
        Task PostAsync<V>(V body, string requesUri, string token = null);
        Task PostAsync(string requesUri, string token = null);
        Task<string> GetAsync(string requesUri, string token = null);
        Task<T> GetAsync<T>(string requesUri, string token = null);
        Task<T> GetAsync<T>(string requesUri, string token, params object[] routes);
    }
}
