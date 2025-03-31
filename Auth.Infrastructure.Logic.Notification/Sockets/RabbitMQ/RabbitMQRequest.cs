using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Auth.Infrastructure.Logic.Notification.Sockets.RabbitMQ
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

            var body = JsonConvert.SerializeObject(message).ToByteArray();
            var properties = new BasicProperties
            {
                Persistent = true
            };
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, 
                mandatory: true, basicProperties: properties, body: body);
        }
    }
}
