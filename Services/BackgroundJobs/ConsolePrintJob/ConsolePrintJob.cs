namespace hosted.services.examples.Services.BackgroundJobs;

public class ConsolePrintJob : IJob
{
    public ConsolePrintJob(string message)
    {
        Message = message;
    }
    public string Label => nameof(ConsolePrintJob);
        
    public string Message { get; init; }
}