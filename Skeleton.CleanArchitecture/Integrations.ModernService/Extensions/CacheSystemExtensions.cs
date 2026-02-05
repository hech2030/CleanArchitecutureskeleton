using Integrations.ModernService.Domain.Entities.Common.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Integrations.ModernService.Extensions;
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
                    options.FailSafeActivationLogLevel = LogLevel.Debug;
                    options.SerializationErrorsLogLevel = LogLevel.Warning;
                    options.FactorySyntheticTimeoutsLogLevel = LogLevel.Debug;
                    options.FactoryErrorsLogLevel = LogLevel.Error;
                })
                .WithDefaultEntryOptions(new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(10),
                    AllowBackgroundBackplaneOperations = true,
                    IsFailSafeEnabled = true,
                    FailSafeMaxDuration = TimeSpan.FromMinutes(10),
                    FailSafeThrottleDuration = TimeSpan.FromMinutes(10),
                    JitterMaxDuration = TimeSpan.FromMinutes(10)
                });

        return services;
    }
}