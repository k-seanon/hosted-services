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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var appRunTask = app.RunAsync();

/* the closure problem */
// var taskqueue = app.Services.GetRequiredService<IBackgroundTaskQueue>();
// var context = new DefaultHttpContext();
// await taskqueue.QueueBackgroundTask(async token =>
// {
//     await Task.Delay(100);
//     var name = context.User?.Identity?.Name ?? "not auth";
//      Console.WriteLine($"Did a thing {name}");
// });
// context = null;


//the better way, your job is a dto, you can store or pass this as defined by your queue needs
//this is leverages strong typed serialization the background service will find a registered handler for the job
//supports multiple handlers
var jobqueue = app.Services.GetRequiredService<IBackgroundJobQueue>();
await jobqueue.Queue(new ConsolePrintJob("happy happy, joy joy"));

await appRunTask;