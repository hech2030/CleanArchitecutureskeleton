using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Skeleton.CleanArchitecture.Extensions;
public static class ResilienceHandlerExtensions
{
    private const int MaxRetry = 4;
    public static void AddStanderResilienceHandler(this IHttpClientBuilder builder)
    {
        builder.AddResilienceHandler("resilience-pipeline", options =>
        {
            // Refer to https://www.pollydocs.org/strategies/retry.html#defaults for retry defaults
            options.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = MaxRetry,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential
            });

            // Refer to https://www.pollydocs.org/strategies/timeout.html#defaults for timeout defaults
            options.AddTimeout(TimeSpan.FromSeconds(20));
        });
    }
}
