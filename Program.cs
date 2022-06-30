using hosted.services.examples.Services;
using hosted.services.examples.Services.BackgroundJobs;
using hosted.services.examples.Services.BackgroundTasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LoggingPollingServiceSettings>(builder.Configuration.GetSection("LoggingPolling"));
builder.Services.AddScoped<PollingTestProcessor>();
builder.Services.AddHostedService<LoggingPollingService>();

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<BackgroundTaskService>();


builder.Services.AddSingleton<IBackgroundJobQueue,InMemoryBackgroundJobQueue>();
builder.Services.AddHostedService<BackgroundJobService>();
builder.Services.AddTransient<IJobHandler<ConsolePrintJob>, ConsolePrintJobHandler>();

var app = builder.Build();


var taskqueue = app.Services.GetRequiredService<IBackgroundTaskQueue>();
var context = new DefaultHttpContext();
await taskqueue.QueueBackgroundTask(token =>
{
    var name = context?.User?.Identity?.Name ?? "not auth";
     Console.WriteLine($"Did a thing {name}");
     return ValueTask.CompletedTask;
});

var jobqueue = app.Services.GetRequiredService<IBackgroundJobQueue>();
await jobqueue.Queue(new ConsolePrintJob("happy happy, joy joy"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
