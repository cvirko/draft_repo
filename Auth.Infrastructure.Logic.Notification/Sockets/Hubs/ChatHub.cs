using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Logic.Models.Hub;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Domain.Interface.Logic.Notification.Sockets.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace Auth.Infrastructure.Logic.Notification.Sockets.Hubs
{
    [SignalRHub]
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [Authorize(AuthConsts.IS_USER)]
    internal class ChatHub(ILogger<ChatHub> logger, ICacheRepository cache) : Hub<IChatHubClient>
    {
        private readonly ILogger _logger = logger;
        private readonly ICacheRepository _cache = cache;

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.GetUserId();
            var connection = await _cache.GetDataAsync<UserHubConnection>(_cache.GetConnectionKey(userId));
            LogInfo(nameof(OnConnectedAsync), userId);
            await UpdateGroupAsync(connection, Context.ConnectionId, userId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.GetUserId();
            if (userId == Guid.Empty)
            {
                userId = await _cache.GetDataAsync<Guid>(_cache.GetConnectionKey(Context.ConnectionId));
            }
            if (userId != Guid.Empty)
            {
                LogInfo(nameof(OnDisconnectedAsync), userId);
                var connection = await _cache.GetDataAsync<UserHubConnection>(_cache.GetConnectionKey(userId));
                connection.DisconnectedDate = DateTimeExtension.Get();
                await _cache.SetDataAsync(_cache.GetConnectionKey(userId), connection);
            }
            await base.OnDisconnectedAsync(exception);
        }
        private async Task UpdateGroupAsync(UserHubConnection connection, string connectionId, Guid userId)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            if (connection?.ConnectionId == connectionId) return;

            if (connection == null)
            {
                connection = new(connectionId);
            }
            foreach (var group in connection.Groups)
            {
                await Groups.RemoveFromGroupAsync(connection.ConnectionId, group);
                await Groups.AddToGroupAsync(connectionId, group);
                LogInfo(nameof(UpdateGroupAsync), userId, group);
            }
            await _cache.RemoveDataAsync(_cache.GetConnectionKey(connection.ConnectionId));
            connection.ConnectionId = connectionId;
            connection.DisconnectedDate = null;
            await _cache.SetDataAsync(_cache.GetConnectionKey(userId), connection);
            await _cache.SetDataAsync(_cache.GetConnectionKey(connection.ConnectionId), userId);
        }
        private void LogInfo(string from, Guid userId, string group = "")
        {
            string infoFormat = string.Format("{0}.{1}: {2}; User: {3};", nameof(ChatHub), from, group, userId);
            _logger.LogInformation(infoFormat);
        }
    }
}
