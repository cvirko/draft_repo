using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.Logic.Notification.Sockets.RabbitMQ
{
    internal class RabbitMQListener(IRabbitMQConnection connection) : IRabbitMQListener
    {
        private readonly IRabbitMQConnection _connection = connection;

        public async Task ReceiveAsync<T>(string queue, Func<T,Task> operationAsync)
        {
            var chanel = await _connection.AddChannelAsync();
            await chanel.QueueDeclareAsync(queue: queue, 
                durable: true, exclusive: false, autoDelete: false, arguments: null);
            //не отправлять более одного сообщения работнику за раз
            await chanel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new AsyncEventingBasicConsumer(chanel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var jsonMessage = ea.Body.ToUTF8String();
                var message = JsonConvert.DeserializeObject<T>(jsonMessage);
                await operationAsync(message);
                //подтверждение принятия сообщения
                await chanel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await chanel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer);
        }
    }
}
