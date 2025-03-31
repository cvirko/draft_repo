using Auth.Client.ConsoleApp.Models.Hubs;

namespace Auth.Client.ConsoleApp.Interfaces.Hubs
{
    internal interface IChatHubClient
    {
        Task ReceiveChatMessageAsync(UserMessageRequest response);
    }
}
