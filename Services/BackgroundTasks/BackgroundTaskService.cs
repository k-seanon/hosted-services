namespace hosted.services.examples.Services.BackgroundTasks;

public class BackgroundTaskService : BackgroundService
{
    private readonly IBackgroundTaskQueue _queue;
    private readonly ILogger _logger;

    public BackgroundTaskService(IBackgroundTaskQueue queue, ILogger<BackgroundTaskService> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("cycling the background task queue");
            try
            {
                Func<CancellationToken, ValueTask> task = await _queue.DequeueAsync(stoppingToken);
                await task.Invoke(stoppingToken);
            }
            catch (Exception ex)
            {
                //catch all, throwing here will prevent all tasks from processing, log and continue
                _logger.LogError(ex, "Error processing Task");
            }
        }
    }
}
