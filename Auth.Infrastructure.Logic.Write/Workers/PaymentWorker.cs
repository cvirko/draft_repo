using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.Queues;
using Auth.Domain.Core.Logic.Commands.Transactions;
using Auth.Domain.Core.Logic.CommandsResponse.Payments;
using Auth.Domain.Interface.Data.Read.Queues;
using Auth.Domain.Interface.Data.Read.UOW;
using Auth.Domain.Interface.Logic.External.Payments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Logic.Write.Workers
{
    internal class PaymentWorker(ILogger<PaymentWorker> logger,
        IQueueRepository<WaitingPaymentApproval> queue,
        IServiceScopeFactory scopeFactory, IConfiguration configuration,
        IHostEnvironment environment) : BackgroundService
    {
        private readonly ILogger<PaymentWorker> _logger = logger;
        private readonly IQueueRepository<WaitingPaymentApproval> _queue = queue;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly PaymentOptions _option = configuration
            .GetSection(AppConsts.PAYMENT_SECTION_NAME).Get<PaymentOptions>();
        private readonly bool IsDevelopment = environment.IsDevelopment();
        private TimeSpan BaseTime => TimeSpan.FromMinutes(_option.FrequencyOfTransactionApprovalChecksInMinutes);
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await TryRunAsync(FillQueueAsync, stoppingToken);
            await TryRunAsync(PaymentsWebhookConfigurationAsync, stoppingToken);
            _ = Task.Run(() => RecurinJobAsync(stoppingToken), stoppingToken).ContinueWith(Log, stoppingToken);
        }
        private void Log(Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    _logger.LogInformation($"Completed");
                    break;
                case TaskStatus.Canceled:
                    _logger.LogWarning($"Canceled");
                    break;
                case TaskStatus.Faulted:
                    _logger.LogError(task.Exception?.InnerException?.Message, task.Exception);
                    break;
            }
        }
        private async Task PaymentsWebhookConfigurationAsync(CancellationToken stoppingToken)
        {
            if (IsDevelopment) return;
            using (var scope = _scopeFactory.CreateScope())
            {
                var _uowPayment = scope.ServiceProvider
                        .GetRequiredService<IPaymentUnitOfWork>();
                await _uowPayment.CreateWebhookConfigurationsAsync();
            }
        }
        private async Task FillQueueAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                using (var uow = scope.ServiceProvider
                    .GetRequiredService<IUnitOfWorkRead>())
                {
                    await foreach(var transaction in
                        uow.Transaction().GetPendingAsync(_option.ExpirePendingInMinutes)
                        .WithCancellation(stoppingToken))
                    {
                        _queue.Add(transaction);
                    }
                }
            }
        }
        private async Task<TimeSpan> CheckUnprocessedTransactionsAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _handler = scope.ServiceProvider.GetRequiredService
                    <ICommandHandler
                        <
                            CheckPaymentTransactionCommand, 
                            CheckPaymentTransactionResponse
                        >
                    >();
                return await NextTransactionsCheckAsync(_handler, stoppingToken);
            }
        }
        private async Task<TimeSpan> NextTransactionsCheckAsync(
            ICommandHandler<CheckPaymentTransactionCommand,
                CheckPaymentTransactionResponse> _handler,
            CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
                return BaseTime;
            var result = await _handler.HandleAsync(new());
            if (!result.IsSuccess)
                return result.Delay;
            return await NextTransactionsCheckAsync(_handler, stoppingToken);
        }
        
        private async Task TryRunAsync(Func<CancellationToken, Task> func, CancellationToken stoppingToken)
        {
            try
            {
                await func(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError(nameof(OperationCanceledException));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Source);
            }
        }
        private async Task RecurinJobAsync(CancellationToken stoppingToken)
        {
            TimeSpan repiet = BaseTime;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    repiet = await CheckUnprocessedTransactionsAsync(stoppingToken);
                    _logger.LogInformation($"{ConsoleConst.GREEN}Worked: Payment time {DateTime.UtcNow}. Next {repiet.TotalMinutes} minutes {ConsoleConst.NORMAL}");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogError(nameof(OperationCanceledException));
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
                await Task.Delay(repiet, stoppingToken);
            }
        }
    }
}
