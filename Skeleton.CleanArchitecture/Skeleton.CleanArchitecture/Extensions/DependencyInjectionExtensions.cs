using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;

namespace Skeleton.CleanArchitecture.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        services.ConfigureAndValidateSingleton<MicrosoftIdentityOptions>(configuration.GetSection("AzureAd"));
        services.ConfigureAndValidateSingleton<DatadogOptions>(configuration.GetSection(nameof(DatadogOptions)));
        services.ConfigureAndValidateSingleton<FusionCacheConfiguration>(configuration.GetSection(FusionCacheConfiguration.CustomRoutingCacheOptions));
        services.ConfigureAndValidateSingleton<ExternalEndpointConfiguration>(configuration.GetSection(nameof(ExternalEndpointConfiguration)));
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
