using Auth.Client.ConsoleApp.Extensions;
using Auth.Client.ConsoleApp.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Auth.Client.ConsoleApp.Services.RabbitMQ
{
    internal class RabbitMQListener(IRabbitMQConnection connection) : IRabbitMQListener
    {
        private readonly IRabbitMQConnection _connection = connection;

        public async Task ReceiveAsync<T>(string queue, Func<T, Task> operationAsync)
        {
            var chanel = await _connection.AddChannelAsync();
            await chanel.QueueDeclareAsync(queue: queue,
                durable: true, exclusive: false, autoDelete: false, arguments: null);
            //not send more than one message to an employee at a time
            await chanel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new AsyncEventingBasicConsumer(chanel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var jsonMessage = ea.Body.ToUTF8String();
                var message = JsonSerializer.Deserialize<T>(jsonMessage);
                await operationAsync(message);
                //acknowledgement of receipt of message
                await chanel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await chanel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer);
        }
    }
}
