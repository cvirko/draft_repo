using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Extensions;
using Auth.Client.ConsoleApp.Interfaces.Actions;
using Auth.Client.ConsoleApp.Interfaces.RabbitMQ;
using Auth.Client.ConsoleApp.Models.ConsoleMessages;
using Auth.Client.ConsoleApp.Services.RabbitMQ;
using Auth.Client.ConsoleApp.Tools;

namespace Auth.Client.ConsoleApp.Services.Actions
{
    internal class RabbitMQActionService : IActionService
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IRabbitMQListener _listener;
        private readonly IRabbitMQRequest _request;
        private SMessage[] messages =
            [
                new(true, "Press 'F' to exit"),
                new(true, "Press 'R' to return"),
                new(true, "Press 'M' to send message"),
            ];
        public RabbitMQActionService(RabbitMQOptions option)
        {
            _connection = new RabbitMQConnection(option);
            _listener = new RabbitMQListener(_connection);
            _listener.ReceiveAsync<string>(AppConsts.APP_NAME, OutPutConsole);
            _request = new RabbitMQRequest(_connection);

        }
        public async Task AddActionsAsync(Func<Task> returnActionAsync)
        {
            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.F)
            {
                for (int i = 0; i < messages.Length; i++)
                    if (messages[i].IsVisible) Console.WriteLine(messages[i].Text);

                key = Console.ReadKey().Key;
                Console.WriteLine("\n");
                switch (key)
                {
                    case ConsoleKey.F:
                        break;
                    case ConsoleKey.R:
                        await returnActionAsync();
                        break;
                    case ConsoleKey.M:
                        Console.WriteLine("input message \n");
                        var message = Console.ReadLine();
                        await _request.SendAsync(AppConsts.APP_NAME, message);
                        break;
                    default: break;
                }
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();

        }

        private Task OutPutConsole(string message)
        {
            ConsoleExtension.Info(message);
            return Task.CompletedTask;
        }
    }
}
