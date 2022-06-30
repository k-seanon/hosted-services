namespace hosted.services.examples.Services.BackgroundJobs
{
    public interface IJob
    {
        string Label { get; }
    }

    public interface IJobHandler
    {
        ValueTask Run(object job, CancellationToken cancellationToken);
    }
    
    public interface IJobHandler<TJob> : IJobHandler
        where TJob : class, IJob
    {
        new ValueTask Run(TJob job, CancellationToken cancellationToken);
    }
    
    public class ConsolePrintJob : IJob
    {
        public ConsolePrintJob(string message)
        {
            Message = message;
        }
        public string Label => nameof(ConsolePrintJob);
        
        public string Message { get; init; }
    }

    public class ConsolePrintJobHandler : BaseJobHandler<ConsolePrintJob>
    {
        private readonly ILogger<ConsolePrintJobHandler> _logger;

        public ConsolePrintJobHandler(ILogger<ConsolePrintJobHandler> logger)
        {
            _logger = logger;
        }

        public override ValueTask Run(ConsolePrintJob job, CancellationToken cancellationToken)
        {
            _logger.LogInformation(job.Message);
            return ValueTask.CompletedTask;
        }

        public new ValueTask Run<T>(T job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BaseJobHandler<TJob> : IJobHandler<TJob> where TJob : class, IJob
    {
        public abstract ValueTask Run(TJob job, CancellationToken cancellationToken);


        public ValueTask Run(object job, CancellationToken cancellationToken) => Run(job as TJob, cancellationToken);
    }
}