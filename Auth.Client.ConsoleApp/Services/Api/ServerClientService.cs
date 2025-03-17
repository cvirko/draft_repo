using Auth.Client.ConsoleApp.Interfaces;
using Auth.Domain.Core.Common.Exceptions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Auth.Client.ConsoleApp.Services.Api
{
    internal class ServerClientService(string uri): IServerClientService
    {
        private HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri(uri)
        };
        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<T> PostAsync<T, V>(V body, string requesUri, string token = null)
        {
            SetToken(token);
            JsonContent content = JsonContent.Create(body);
            var result = await _client.PostAsync(requesUri, content);
            await EnsureSuccessStatusCodeAsync(result);
            return await ConvertAsync<T>(result.Content);
        }
        public async Task PostAsync<V>(V body, string requesUri, string token = null)
        {
            SetToken(token);
            JsonContent content = JsonContent.Create(body);
            var result = await _client.PostAsync(requesUri, content);
            await EnsureSuccessStatusCodeAsync(result);
        }
        public async Task PostAsync(string requesUri, string token = null)
        {
            SetToken(token);
            var result = await _client.PostAsync(requesUri, null);
            await EnsureSuccessStatusCodeAsync(result);
        }

        public async Task<string> GetAsync(string requesUri, string token = null)
        {
            SetToken(token);
            var result = await _client.GetAsync(requesUri);
            await EnsureSuccessStatusCodeAsync(result);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<T> GetAsync<T>(string requesUri, string token = null)
        {
            SetToken(token);
            var result = await _client.GetAsync(requesUri);
            await EnsureSuccessStatusCodeAsync(result);
            return await ConvertAsync<T>(result.Content);
        }
        public async Task<T> GetAsync<T>(string requesUri, string token, params object[] routes)
        {
            string routesParams = null;
            if (routes != null && routes.Length > 0)
                routesParams = "/" + string.Join("/", routes);
            SetToken(token);
            var result = await _client.GetAsync(requesUri + routesParams);
            await EnsureSuccessStatusCodeAsync(result);
            return await ConvertAsync<T>(result.Content);
        }
        private void SetToken(string token)
        {
            if (string.IsNullOrEmpty(token) || token == _client.DefaultRequestHeaders.Authorization?.Parameter)
                return;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        private async Task<T> ConvertAsync<T>(HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
        }
        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;
            var asts = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ErrorResponse>(asts);
            throw new ResponseException(result?.Message, response.StatusCode, result?.ValidationErrors ?? [], [result?.Detail]);
        }
    }
}
