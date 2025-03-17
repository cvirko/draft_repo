using Auth.Client.ConsoleApp.Interfaces;

namespace Auth.Client.ConsoleApp.Services.Api
{
    public class ChatApiService(IServerClientService client) : IChatApiService
    {
        private IServerClientService _client = client;

        private Dictionary<Requests, string> requestPath = new()
        {
            {Requests.Join, "/api/v1/Chat/Join/{0}" },
            {Requests.Leave,"/api/v1/Chat/Leave/{0}" },
            {Requests.Send,"/api/v1/Chat/Send/Message/{0}" },
        };
        public async Task JoinAsync(string name, string token)
        {
            string requestFormated = string.Format(requestPath[Requests.Join], name);
            await _client.PostAsync(requestFormated, token);
        }
        public async Task LeaveAsync(string name, string token)
        {
            string requestFormated = string.Format(requestPath[Requests.Leave], name);
            await _client.PostAsync(requestFormated, token);
        }
        public async Task SendAsync(string name, string token)
        {
            Console.WriteLine("Input text");
            var message = Console.ReadLine();
            string requestFormated = string.Format(requestPath[Requests.Send], name);
            await _client.PostAsync(message, requestFormated, token);
        }
        private enum Requests
        {
            Send,
            Leave,
            Join
        }
    }
}
