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

        public async Task<IChannel> AddChannelAsync()
        {
            await TryConnectAsync();
            if (Channel != null && Channel.IsOpen) return Channel;

            var mylock = await _locks.GetLockAsync(ChannelName);
            try
            {
                await TryAddChannelAsync();
            }
            finally
            {
                mylock.Release();
            }
            return Channel;
        }

        public void Dispose()
        {
            var chanelTask = Channel?.CloseAsync();
            var connectionTask = Connection?.CloseAsync();
            Task.WhenAll(chanelTask, connectionTask).ContinueWith(_ =>
            {
                Channel?.Dispose();
                Connection?.Dispose();
                _locks.Remove(ChannelName);
            });
        }
        private async Task<bool> TryConnectAsync()
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
            Connection = await factory.CreateConnectionAsync(endpoints);
            return true;
        }
        private async Task<bool> TryAddChannelAsync()
        {
            if (Connection == null) return false;
            if (Channel != null && Channel.IsOpen) return false;

            Channel = await Connection.CreateChannelAsync();
            return true;
        }
    }
}
