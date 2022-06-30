namespace hosted.services.examples.Services.Common;

public class ScopedBackgroundProcessingService<TProcessor>
    : BackgroundService
    where TProcessor : IScopedProcessor
{
    protected readonly IServiceScopeFactory _serviceScopeFactory;

    public ScopedBackgroundProcessingService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<TProcessor>();
        return processor.Execute(stoppingToken);
    }
}
