using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Skeleton.CleanArchitecture.Extensions;
internal static class CacheSystemExtensions
{
    public static IServiceCollection AddCacheSystemInjection(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IHostEnvironment hostingEnvironment)
    {
        var fusionCacheConfiguration = configuration.GetSection(FusionCacheConfiguration.DefaultFusionCacheConfiguration).Get<FusionCacheConfiguration>();


        //This is a simple cache, you can use a second level cache (REDIS)
        services.AddFusionCache()
                .WithOptions(options =>
                {
                    options.DistributedCacheCircuitBreakerDuration = TimeSpan.FromSeconds(2);
                    options.FailSafeActivationLogLevel = LogLevel.Debug;
                    options.SerializationErrorsLogLevel = LogLevel.Warning;
                    options.DistributedCacheSyntheticTimeoutsLogLevel = LogLevel.Debug;
                    options.DistributedCacheErrorsLogLevel = LogLevel.Error;
                    options.FactorySyntheticTimeoutsLogLevel = LogLevel.Debug;
                    options.FactoryErrorsLogLevel = LogLevel.Error;
                })
                .WithDefaultEntryOptions(new FusionCacheEntryOptions
                {
                    Duration = fusionCacheConfiguration!.Duration,
                    AllowBackgroundDistributedCacheOperations = true,
                    AllowBackgroundBackplaneOperations = true,
                    IsFailSafeEnabled = true,
                    FailSafeMaxDuration = fusionCacheConfiguration!.FailSafeMaxDuration,
                    FailSafeThrottleDuration = fusionCacheConfiguration!.FailSafeThrottleDuration,
                    JitterMaxDuration = fusionCacheConfiguration!.JitterMaxDuration
                });

        return services;
    }
}