using Microsoft.Extensions.Diagnostics.HealthChecks;
using StatsdClient;

namespace Skeleton.CleanArchitecture.Health
{
    internal static class HealthCheckServicesExtensions
    {
        public static IServiceCollection AddHealthCheckServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .Configure<HealthCheckPublisherOptions>(configuration.GetRequiredSection(nameof(HealthCheckPublisherOptions)));

            services.AddSingleton<DogStatsdService>();

            services
                .AddHealthChecks()
                .AddCheck(name: "Self", check: () =>
                    {
                    return HealthCheckResult.Healthy("Self check");
                })
                .AddDatadogPublisher(
                    serviceCheckName: "Skeleton.CleanArchitecture.healthchecks"
                );

            return services;
        }
    }
}
