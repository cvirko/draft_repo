using Auth.Client.ConsoleApp.Interfaces.RabbitMQ;
using Auth.Client.ConsoleApp.Tools;
using RabbitMQ.Client;

namespace Auth.Client.ConsoleApp.Services.RabbitMQ
{
    internal class RabbitMQConnection(RabbitMQOptions options) : IRabbitMQConnection
    {
        private readonly RabbitMQOptions _options = options;
        private IChannel Channel;
        private IConnection Connection;

        public async Task<IChannel> AddChannelAsync()
        {
            await TryConnectAsync();
            if (Channel != null && Channel.IsOpen) return Channel;

            await TryAddChannelAsync();
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
