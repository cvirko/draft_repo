using Auth.Client.ConsoleApp.Extensions;
using Auth.Client.ConsoleApp.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using System.Text.Json;

namespace Auth.Client.ConsoleApp.Services.RabbitMQ
{
    internal class RabbitMQRequest(IRabbitMQConnection connection) : IRabbitMQRequest
    {
        private readonly IRabbitMQConnection _connection = connection;

        public async Task SendAsync<T>(string queue, T message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var channel = await _connection.AddChannelAsync();
            await channel.QueueDeclareAsync(queue: queue,
                durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            var body = JsonSerializer.Serialize(message).ToByteArray();

            var properties = new BasicProperties
            {
                Persistent = true
            };
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue,
                mandatory: true, basicProperties: properties, body: body);
        }
    }
}
