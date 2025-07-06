using Aptiverse.Worker.Clients;
using Aptiverse.Worker.Models;

namespace Aptiverse.Worker.Services
{
    public class MessageProcessor(FastApiClient apiClient)
    {
        public async Task ProcessAsync(TaskPayload payload)
        {
            switch (payload.TaskType?.ToLowerInvariant())
            {
                case "summarization":
                    await apiClient.SendToSummarizer(payload);
                    break;
                case "emotion":
                    await apiClient.SendToEmotionAnalysis(payload);
                    break;
                default:
                    throw new NotSupportedException($"Unknown task type: {payload.TaskType}");
            }
        }
    }
}
