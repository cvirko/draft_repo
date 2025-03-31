namespace Auth.Client.ConsoleApp.Interfaces.RabbitMQ
{
    internal interface IRabbitMQRequest
    {
        Task SendAsync<T>(string queue, T message);
    }
}
