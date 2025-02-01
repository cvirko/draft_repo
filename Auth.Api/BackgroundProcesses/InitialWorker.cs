using Auth.Domain.Core.Logic.Commands.Admin;

namespace Auth.Api.BackgroundProcesses
{
    public class InitialWorker(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run(() => InfiniteLoopAsync(RemoveUserTokensAsync,TimeSpan.FromDays(1), stoppingToken), stoppingToken);
            await Task.FromResult(stoppingToken);
        }
        private async Task RemoveUserTokensAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var command = scope.ServiceProvider.GetRequiredService<ICommandHandler<RemoveUselessTokensCommand>>();
                await command.HandleAsync(new RemoveUselessTokensCommand { Token = stoppingToken });
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
