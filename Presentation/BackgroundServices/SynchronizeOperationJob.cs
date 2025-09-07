
using Application.Payment;

namespace Presentation.BackgroundServices;

/// <summary>
/// Job that synchronize our operations with external ones (in case we did not get a webhook)
/// </summary>
/// <exception cref="Exception">Might throw an exception, it will be ignored</exception>
public sealed class SynchronizeOperationJob(
    ILogger<SynchronizeOperationJob> _logger,
    IServiceScopeFactory _factory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Started synchronization job at: {StartTime}", DateTime.UtcNow);

            await using var scope = _factory.CreateAsyncScope();

            var service = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                
            await service.SynchronizeOperationsAsync(stoppingToken);

            _logger.LogInformation("Synchronization job done at: {StartTime}", DateTime.UtcNow);
            
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}