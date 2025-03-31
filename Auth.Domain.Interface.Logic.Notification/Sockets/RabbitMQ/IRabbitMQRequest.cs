namespace Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ
{
    public interface IRabbitMQRequest
    {
        Task SendAsync<T>(string queue, T message);
    }
}
