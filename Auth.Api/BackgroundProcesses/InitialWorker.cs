using Auth.Domain.Core.Logic.Commands.Admin;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;

namespace Auth.Api.BackgroundProcesses
{
    public class InitialWorker(IServiceScopeFactory scopeFactory, IRabbitMQListener rabbitMQListener) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly IRabbitMQListener _rabbitMQListener = rabbitMQListener;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run(() => InfiniteLoopAsync(RemoveUserTokensAsync,TimeSpan.FromDays(1), stoppingToken), stoppingToken);
            await TryRunAsync();
            await Task.FromResult(stoppingToken);
            ConsoleExtension.Errors("InitialWorker ends");
        }
        private async Task RemoveUserTokensAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var command = scope.ServiceProvider.GetRequiredService<ICommandHandler<RemoveUselessTokensCommand>>();
                await command.HandleAsync(new RemoveUselessTokensCommand { Token = stoppingToken });
            }
        }
        private Task OutPutConsole<T>(T message)
        {
            ConsoleExtension.Info(message, ConsoleColor.Green);
            return Task.CompletedTask;
        }
        private async Task TryRunAsync()
        {
            try
            {
                await _rabbitMQListener.ReceiveAsync<string>(AppConsts.APP_NAME, OutPutConsole);
            }
            catch (Exception ex)
            {
                ConsoleExtension.Errors($"Failed to run: {ex.Source} {ex.Message}");
                Console.Error.WriteLine(ex);
            }
        }
        private async Task InfiniteLoopAsync(Func<CancellationToken, Task> func, TimeSpan repiet, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await func(stoppingToken);
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine(ex.Message,ex);
                }
                await Task.Delay(repiet, stoppingToken);
            }
        }
    }
}
