using Aptiverse.Worker;
using Aptiverse.Worker.Clients;
using Aptiverse.Worker.Config;
using Aptiverse.Worker.Consumers;
using Aptiverse.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

// Bind RabbitMQ & FastAPI settings from appsettings.json
builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.Configure<FastApiSettings>(
    builder.Configuration.GetSection("FastApi")
);

// Register services
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddSingleton<MessageProcessor>();
builder.Services.AddHttpClient<FastApiClient>();

// Register worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
