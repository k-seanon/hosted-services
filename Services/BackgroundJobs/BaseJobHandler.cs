namespace hosted.services.examples.Services.BackgroundJobs;

public abstract class BaseJobHandler<TJob> : IJobHandler<TJob> where TJob : class, IJob
{
    public abstract ValueTask Run(TJob job, CancellationToken cancellationToken);


    public ValueTask Run(object job, CancellationToken cancellationToken) => Run(job as TJob, cancellationToken);
}