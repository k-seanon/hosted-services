namespace hosted.services.examples.Services.BackgroundJobs;

public class ConsolePrintJobHandler : BaseJobHandler<ConsolePrintJob>
{
    private readonly ILogger<ConsolePrintJobHandler> _logger;

    public ConsolePrintJobHandler(ILogger<ConsolePrintJobHandler> logger)
    {
        _logger = logger;
    }

    public override ValueTask Run(ConsolePrintJob job, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{job.Label}] {job.Message}");
        return ValueTask.CompletedTask;
    }
}