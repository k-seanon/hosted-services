using Cronos;

namespace hosted.services.examples.Services.ScheduledTaskService;

public class CronTaskService : BackgroundService
{
    private readonly CronExpression _expression = CronExpression.Parse("*/5 * * * * *", CronFormat.IncludeSeconds);
    private Timer _timer;

    public CronTaskService()
    {
        
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await ScheduleJob();

        await base.StartAsync(cancellationToken);
    }

    private async Task ScheduleJob()
    {
        var next = _expression.GetNextOccurrence(DateTime.Now, inclusive: true);

        if(next.HasValue)
        {
            var delay = next.Value - DateTimeOffset.Now;
            if(delay.TotalMilliseconds <= 0)
            {
                await ScheduleJob();
            }

            _timer = new Timer(Tick, state: null, TimeSpan.Zero, delay);
        }
    }

    private void Tick(object? state)
    {
        _timer.Dispose();
        _timer = null;


    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
