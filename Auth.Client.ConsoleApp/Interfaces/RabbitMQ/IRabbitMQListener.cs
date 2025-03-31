namespace Auth.Client.ConsoleApp.Interfaces.RabbitMQ
{
    internal interface IRabbitMQListener
    {
        Task ReceiveAsync<T>(string queue, Func<T, Task> operationAsync);
    }
}
