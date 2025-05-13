using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.Logic.Notification.Sockets.RabbitMQ
{
    internal class RabbitMQListener(IRabbitMQConnection connection,
        IOptions<RabbitMQOptions> options) : IRabbitMQListener
    {
        private readonly IRabbitMQConnection _connection = connection;
        private readonly RabbitMQOptions _options = options.Value;
        public async Task ReceiveAsync<T>(string queue, Func<T,Task> operationAsync, CancellationToken stoppingToken)
        {
            if (!_options.IsUseListener)
                return;
            var chanel = await _connection.AddChannelAsync(stoppingToken);
            await chanel.QueueDeclareAsync(queue: queue, 
                durable: true, exclusive: false, autoDelete: false, arguments: null,cancellationToken: stoppingToken);
            //not send more than one message to an employee at a time
            await chanel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, stoppingToken);
            var consumer = new AsyncEventingBasicConsumer(chanel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var jsonMessage = ea.Body.ToUTF8String();
                var message = JsonConvert.DeserializeObject<T>(jsonMessage);
                await operationAsync(message);
                //acknowledgement of receipt of message
                await chanel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, stoppingToken);
            };

            await chanel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, stoppingToken);
        }
    }
}
