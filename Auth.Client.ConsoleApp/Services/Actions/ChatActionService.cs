using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Interfaces.Actions;
using Auth.Client.ConsoleApp.Interfaces.Api;
using Auth.Client.ConsoleApp.Models.ConsoleMessages;
using Auth.Client.ConsoleApp.Services.SignalR;

namespace Auth.Client.ConsoleApp.Services.Actions
{
    internal class ChatActionService : IActionService
    {
        private SignalRClient _socket;
        private IUnitOfWorkApi _api;
        public ChatActionService(IUnitOfWorkApi api, string uri)
        {
            _socket = new SignalRClient(string.Join("/", uri, AppConsts.HUBNAME), api);
            _ = _socket.ConnectToMessageListener();
            IUnitOfWorkApi _api = api;
        }
        
        private SMessage[] messages = [
                new(true, "Press 'F' to exit"),
                new(true, "Press 'R' to return"),
                new(false, "Press 'D' to disconnect"),
                new(true, "Press 'J' to join"),
                new(false, "Press 'M' to send message"),
                new(true, "Press 'G' to change group name"),
            ];
        private string groupName = "test";
        public async Task AddActionsAsync(Func<Task> returnActionAsync)
        {
            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.F)
            {
                for (int i = 0; i < messages.Length; i++)
                    if (messages[i].IsVisible) Console.WriteLine(messages[i].Text);

                key = Console.ReadKey().Key;
                Console.WriteLine("\n");
                switch (key)
                {
                    case ConsoleKey.F:
                        break;
                    case ConsoleKey.R:
                        await returnActionAsync();
                        break;
                    case ConsoleKey.G:
                        Console.WriteLine("Input group name");
                        groupName = Console.ReadLine();
                        break;
                    case ConsoleKey.D:
                        await _api.Chat().LeaveAsync(groupName, _api.Account().Token);
                        messages[2].IsVisible = false;
                        messages[4].IsVisible = false;
                        messages[3].IsVisible = true;
                        messages[5].IsVisible = true;
                        break;
                    case ConsoleKey.J:
                        await _api.Chat().JoinAsync(groupName, _api.Account().Token);
                        messages[2].IsVisible = true;
                        messages[4].IsVisible = true;
                        messages[3].IsVisible = false;
                        messages[5].IsVisible = false;
                        break;
                    case ConsoleKey.M:
                        await _api.Chat().SendAsync(groupName, _api.Account().Token);
                        break;
                    default: break;
                }
            }
        }

        public void Dispose()
        {
            _socket?.Dispose();
            messages = null;
        }
    }
}
