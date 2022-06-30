namespace hosted.services.examples.Services;

public interface IScopedProcessor
{
    Task Execute(CancellationToken cancellationToken);
}
