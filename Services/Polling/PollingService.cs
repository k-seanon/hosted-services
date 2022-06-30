namespace hosted.services.examples.Services;

public class PollingService<TProcessor> : BackgroundService
    where TProcessor : IScopedProcessor
{

    private readonly PeriodicTimer _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public PollingService(IServiceScopeFactory scopeFactory, IPollingServiceSettings settings)
    {
        _timer = new PeriodicTimer(settings.Interval);
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<TProcessor>();
            await processor.Execute(stoppingToken);
        }
    }
}
