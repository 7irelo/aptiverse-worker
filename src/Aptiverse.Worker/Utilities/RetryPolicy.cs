using Polly;
using Polly.Retry;

namespace Aptiverse.Worker.Utilities
{
    public static class RetryPolicy
    {
        public static AsyncRetryPolicy CreatePolicy() =>
            Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
