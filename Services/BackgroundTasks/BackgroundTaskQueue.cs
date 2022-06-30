using System.Threading.Channels;

namespace hosted.services.examples.Services.BackgroundTasks;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundTaskQueue()
    {
        var options = new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        var task = await _queue.Reader.ReadAsync(cancellationToken);
        return task;
    }

    public async ValueTask QueueBackgroundTask(Func<CancellationToken, ValueTask> task)
    {
        ArgumentNullException.ThrowIfNull(task, nameof(task));

        await _queue.Writer.WriteAsync(task);
    }
}
