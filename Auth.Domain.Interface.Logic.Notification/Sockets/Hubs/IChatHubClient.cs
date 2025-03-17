using Auth.Domain.Core.Logic.Models.Hub;
using SignalRSwaggerGen.Attributes;

namespace Auth.Domain.Interface.Logic.Notification.Sockets.Hubs
{
    [SignalRHub]
    public interface IChatHubClient
    {
        Task ReceiveChatMessageAsync(UserMessageRequest response);
    }
}
