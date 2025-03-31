using RabbitMQ.Client;

namespace Auth.Client.ConsoleApp.Interfaces.RabbitMQ
{
    internal interface IRabbitMQConnection : IDisposable
    {
        public Task<IChannel> AddChannelAsync();
    }
}
