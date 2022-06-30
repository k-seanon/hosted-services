using System.Threading.Channels;

namespace hosted.services.examples.Services.BackgroundJobs;

public interface IBackgroundJobQueue
{
    ValueTask<IJob> DequeueAsync(CancellationToken cancellationToken);
    ValueTask Queue(IJob job);
}

public class InMemoryBackgroundJobQueue : IBackgroundJobQueue
{
    private readonly Channel<IJob> _queue;

    public InMemoryBackgroundJobQueue()
    {
        var options = new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<IJob>(options);
    }

    public async ValueTask<IJob> DequeueAsync(CancellationToken cancellationToken)
    {
        var task = await _queue.Reader.ReadAsync(cancellationToken);
        return task;
    }

    public async ValueTask Queue(IJob job)
    {
        ArgumentNullException.ThrowIfNull(job, nameof(job));
        //it would be wise to move your dto job.
        await _queue.Writer.WriteAsync(job);
    }
}