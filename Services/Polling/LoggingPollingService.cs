using Microsoft.Extensions.Options;

namespace hosted.services.examples.Services;

public class LoggingPollingService : PollingService<PollingTestProcessor>
{
    public LoggingPollingService(IServiceScopeFactory scopeFactory, IOptions<LoggingPollingServiceSettings> options) 
        : base(scopeFactory, options.Value)
    {
    }
}

public class PollingTestProcessor : IScopedProcessor
{
    private readonly ILogger<PollingTestProcessor> _logger;

    public PollingTestProcessor(ILogger<PollingTestProcessor> logger)
    {
        _logger = logger;
    }
    public Task Execute(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Task executing {Processor}", nameof(PollingTestProcessor));
        return Task.CompletedTask;
    }
}
