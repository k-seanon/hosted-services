namespace hosted.services.examples.Services.BackgroundJobs
{
    public interface IJob
    {
        string Label { get; }
    }

    public interface IJobHandler
    {
        ValueTask Run<T>(T job, CancellationToken cancellationToken);
    }
    
    public interface IJobHandler<TJob> : IJobHandler
        where TJob : IJob
    {
        new ValueTask Run<T>(T job, CancellationToken cancellationToken);
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

    public class ConsolePrintJobHandler : IJobHandler<ConsolePrintJob>
    {
        private readonly ILogger<ConsolePrintJobHandler> _logger;

        public ConsolePrintJobHandler(ILogger<ConsolePrintJobHandler> logger)
        {
            _logger = logger;
        }

        public new ValueTask Run(ConsolePrintJob job, CancellationToken cancellationToken)
        {
            _logger.LogInformation(job.Message);
            return ValueTask.CompletedTask;
        }
        
        ValueTask IJobHandler<ConsolePrintJob>.Run<T>(T job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        ValueTask IJobHandler.Run<T>(T job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}