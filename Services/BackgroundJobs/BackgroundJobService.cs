namespace hosted.services.examples.Services.BackgroundJobs
{
    public class BackgroundJobService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBackgroundJobQueue _queue;

        public BackgroundJobService(ILogger<BackgroundJobService> logger, IServiceScopeFactory serviceScopeFactory, IBackgroundJobQueue queue)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _queue = queue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("cycling the background job queue");
                try
                {
                    var job = await _queue.DequeueAsync(stoppingToken);
                    using var scope = _serviceScopeFactory.CreateScope();
                    await Parallel.ForEachAsync(GetService(scope.ServiceProvider, job),
                        async (handler, token) =>
                        {
                            try
                            {
                                await handler.Run(job, token);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(exception: e, message: "Error processing {@Job} with {Handler}", job, handler);
                                throw;
                            }
                        });
                }
                catch (Exception ex)
                {
                    //catch all, throwing here will prevent all tasks from processing, log and continue
                    _logger.LogError(ex, "Error processing Task");
                }
            }
        }

        private IEnumerable<IJobHandler> GetService(IServiceProvider serviceProvider, IJob job)
        {
            var type = typeof(IJobHandler<>);
            var qualified = type.MakeGenericType(job.GetType());

            foreach (var handler in serviceProvider.GetServices(qualified))
            {
                yield return handler as IJobHandler;
            }
        }
    }
}