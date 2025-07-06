using Aptiverse.Worker.Config;
using Aptiverse.Worker.Models;
using Aptiverse.Worker.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;

namespace Aptiverse.Worker.Consumers
{
    public class RabbitMqConsumer(IOptions<RabbitMqSettings> config, MessageProcessor processor)
    {
        private readonly RabbitMqSettings _settings = config.Value;

        public async Task StartListeningAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_settings.Host),
                UserName = _settings.Username,
                Password = _settings.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(_settings.QueueName, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var payload = JsonSerializer.Deserialize<TaskPayload>(message);

                if (payload != null)
                    await processor.ProcessAsync(payload);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(_settings.QueueName, false, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
