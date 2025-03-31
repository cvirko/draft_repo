using RabbitMQ.Client;

namespace Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ
{
    public interface IRabbitMQConnection: IDisposable
    {
        public Task<IChannel> AddChannelAsync();
    }
}
