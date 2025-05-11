using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Interface.Data.Read.Locks;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Auth.Infrastructure.Logic.Notification.Sockets.RabbitMQ
{
    internal class RabbitMQConnection(IOptions<RabbitMQOptions> options, 
        IAsyncSynchronization locks) : IRabbitMQConnection
    {
        private readonly RabbitMQOptions _options = options.Value;
        private IAsyncSynchronization _locks = locks;
        private IChannel Channel;
        private IConnection Connection;
        private const string ChannelName = "RabbitMQ.Channel";

        public async Task<IChannel> AddChannelAsync(CancellationToken stoppingToken = default)
        {
            await TryConnectAsync(stoppingToken);
            if (Channel != null && Channel.IsOpen) return Channel;

            var mylock = await _locks.GetLockAsync(ChannelName);
            try
            {
                await TryAddChannelAsync(stoppingToken);
            }
            finally
            {
                mylock.Release();
            }
            return Channel;
        }
        public void Dispose()
        {
            if (Connection is null) return;

            Task chanelTask = Channel?.CloseAsync() ?? Task.CompletedTask;
            Task connectionTask = Connection?.CloseAsync();
            Task.WhenAll(chanelTask, connectionTask).ContinueWith(_ =>
            {
                Channel?.Dispose();
                Connection?.Dispose();
                _locks.Remove(ChannelName);
            });
        }
        private async Task<bool> TryConnectAsync(CancellationToken stoppingToken = default)
        {
            if (Connection != null && Connection.IsOpen) return false;
            var factory = new ConnectionFactory
            {
                UserName = _options.UserName,
                Password = _options.Password,
                RequestedChannelMax = _options.ChannelsMax,
                ClientProvidedName = _options.ProvidedName,
                AutomaticRecoveryEnabled = true,
            };
            var endpoints = _options.Hosts.Select(p => new AmqpTcpEndpoint(p.Name, p.Port));
            Connection = await factory.CreateConnectionAsync(endpoints, stoppingToken);
            return true;
        }
        private async Task<bool> TryAddChannelAsync(CancellationToken stoppingToken = default)
        {
            if (Connection == null) return false;
            if (Channel != null && Channel.IsOpen) return false;

            Channel = await Connection.CreateChannelAsync(null, stoppingToken);
            return true;
        }
    }
}
