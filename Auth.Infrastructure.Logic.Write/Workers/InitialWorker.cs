using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Logic.Write.Workers
{
    public class InitialWorker(IServiceScopeFactory scopeFactory, ILogger<InitialWorker> logger,
        IRabbitMQListener rabbitMQListener) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly IRabbitMQListener _rabbitMQListener = rabbitMQListener;
        private readonly ILogger<InitialWorker> _logger = logger;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run(() => InfiniteLoopAsync(RemoveUserTokensAsync,TimeSpan.FromDays(1), stoppingToken), stoppingToken);
            await TryRunAsync(stoppingToken);
            _logger.LogInformation("{0}InitialWorker ends{1}", ConsoleConst.GREEN, ConsoleConst.NORMAL);
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
            _logger.LogInformation("{0}{2}{1}", ConsoleConst.GREEN, ConsoleConst.NORMAL, message);
            return Task.CompletedTask;
        }
        private async Task TryRunAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _rabbitMQListener.ReceiveAsync<string>(AppConsts.APP_NAME, OutPutConsole, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"{0}{2}{1}", ConsoleConst.RED, ConsoleConst.NORMAL, ex.Message);
            }
        }
        private async Task InfiniteLoopAsync(Func<CancellationToken, Task> func, TimeSpan repiet, CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(repiet);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await func(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{0}{2}{1}", ConsoleConst.RED, ConsoleConst.NORMAL, ex.Message);
                }
            }
        }
    }
}
