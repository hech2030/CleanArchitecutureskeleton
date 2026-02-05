using Microsoft.Extensions.Options;
using Integrations.ModernService.Domain.Entities.Common.Options;

namespace Integrations.ModernService.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        services.ConfigureAndValidateSingleton<FusionCacheConfiguration>(configuration.GetSection(FusionCacheConfiguration.CustomRoutingCacheOptions));
        services.ConfigureAndValidateSingleton<LinkedInServiceEndpointConfiguration>(configuration.GetSection(nameof(LinkedInServiceEndpointConfiguration)));
        return services;
    }

    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations();
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }
}
