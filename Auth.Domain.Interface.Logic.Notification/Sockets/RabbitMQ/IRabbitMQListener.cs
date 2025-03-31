namespace Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ
{
    public interface IRabbitMQListener
    {
        Task ReceiveAsync<T>(string queue, Func<T, Task> operationAsync);
    }
}
