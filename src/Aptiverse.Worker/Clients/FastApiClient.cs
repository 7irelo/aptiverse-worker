using Aptiverse.Worker.Config;
using Aptiverse.Worker.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Aptiverse.Worker.Clients
{
    public class FastApiClient(HttpClient client, IOptions<FastApiSettings> settings)
    {
        private readonly string _baseUrl = settings.Value.BaseUrl;

        public async Task<HttpResponseMessage> SendToSummarizer(TaskPayload payload)
        {
            return await client.PostAsJsonAsync($"{_baseUrl}/summarize", payload);
        }

        public async Task<HttpResponseMessage> SendToEmotionAnalysis(TaskPayload payload)
        {
            return await client.PostAsJsonAsync($"{_baseUrl}/emotion-analysis", payload);
        }
    }
}
