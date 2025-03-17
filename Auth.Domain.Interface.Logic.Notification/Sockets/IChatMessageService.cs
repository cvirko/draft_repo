using Auth.Domain.Core.Logic.Models.Hub;

namespace Auth.Domain.Interface.Logic.Notification.Sockets
{
    public interface IChatMessageService
    {
        Task SendChatMessageAsync(UserMessage message);
        Task AddUserToGroupAsync(string groupName, Guid userId);
        Task RemoveUserFromGroupAsync(string groupName, Guid userId);
    }
}
