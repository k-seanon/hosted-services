namespace hosted.services.examples.Services;

public interface IPollingServiceSettings
{
    public TimeSpan Interval { get; set; }
}
