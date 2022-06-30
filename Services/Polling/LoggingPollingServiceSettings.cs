namespace hosted.services.examples.Services;

public class LoggingPollingServiceSettings : IPollingServiceSettings
{
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(10);
}
