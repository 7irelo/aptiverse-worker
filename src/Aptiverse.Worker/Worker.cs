using Aptiverse.Worker.Consumers;

namespace Aptiverse.Worker
{
    public class Worker(ILogger<Worker> logger, RabbitMqConsumer consumer) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly RabbitMqConsumer _consumer = consumer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            await _consumer.StartListeningAsync(stoppingToken);
        }
    }
}
