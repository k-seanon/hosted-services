namespace hosted.services.examples.Services.BackgroundTasks;

public interface IBackgroundTaskQueue
{
    ValueTask QueueBackgroundTask(Func<CancellationToken, ValueTask> task);
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}
