using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Logic.Models.Hub;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Domain.Interface.Logic.Notification.Sockets;
using Auth.Domain.Interface.Logic.Notification.Sockets.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Logic.Notification.Sockets.Hubs
{
    internal class ChatMessageService(ILogger<ChatMessageService> logger,
        IHubContext<ChatHub, IChatHubClient> hub, ICacheRepository cache) : IChatMessageService
    {
        private readonly ILogger<ChatMessageService> _logger = logger;
        private readonly IHubContext<ChatHub, IChatHubClient> _hub = hub;
        private readonly ICacheRepository _cache = cache;
        public async Task SendChatMessageAsync(UserMessage message)
        {
            if (string.IsNullOrEmpty(message.Id)) return;
            if (string.IsNullOrEmpty(message.UserName)) return;
            if (string.IsNullOrEmpty(message.GroupName)) return;
            if (string.IsNullOrEmpty(message.Text)) return;
            var connection = await _cache.GetDataAsync<UserConnection>(_cache.GetConnectionKey(message.UserId));
            if (connection?.IsDisconected ?? true
                || !connection.Groups.Contains(message.GroupName))
            {
                throw new ForbiddenException($"No {AppConsts.HUBNAME} connection");
            }

            await _hub.Clients.Group(message.GroupName).ReceiveChatMessageAsync(new(message));
            LogInfo(nameof(SendChatMessageAsync), message.UserId, message.GroupName, message.Text);
        }
        public async Task AddUserToGroupAsync(string groupName, Guid userId)
        {
            if (string.IsNullOrEmpty(groupName)) return;
            if (userId == Guid.Empty) return;
            var connection = await _cache.GetDataAsync<UserConnection>(_cache.GetConnectionKey(userId));
            if (connection?.IsDisconected ?? true ) 
            {
                throw new ForbiddenException($"No {AppConsts.HUBNAME} connection");
            }
            if (!connection.Groups.TryAdd(groupName))
            {
                return;
                throw new ForbiddenException("Already added to group");
            }
            await _hub.Groups.AddToGroupAsync(connection.ConnectionId, groupName);
            await _cache.SetDataAsync(_cache.GetConnectionKey(userId), connection);
            LogInfo(nameof(AddUserToGroupAsync), userId, groupName);
        }
        public async Task RemoveUserFromGroupAsync(string groupName, Guid userId)
        {
            if (string.IsNullOrEmpty(groupName)) return;
            if (userId == Guid.Empty) return;
            var connection = await _cache.GetDataAsync<UserConnection>(_cache.GetConnectionKey(userId));
            if (connection?.IsDisconected ?? true)
            {
                throw new ForbiddenException($"No {AppConsts.HUBNAME} connection");
            }

            if (!connection.Groups.TryRemove(groupName))
            {
                return;
                throw new ForbiddenException("Already removed from group");
            }
            await _hub.Groups.RemoveFromGroupAsync(connection.ConnectionId, groupName);
            await _cache.SetDataAsync(_cache.GetConnectionKey(userId), connection);
            LogInfo(nameof(RemoveUserFromGroupAsync), userId, groupName);
        }
        private void LogInfo(string from, Guid userId, string group, string message = "" )
        {
            string infoFormat = string.Format("{0}.{1}: {2}; User: {3}; Message: {4}", nameof(ChatHub), from, group, userId, message);
            _logger.LogInformation(infoFormat);
        }
    }
}
