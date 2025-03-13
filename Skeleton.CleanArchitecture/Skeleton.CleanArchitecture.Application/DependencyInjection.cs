using Microsoft.Extensions.DependencyInjection;

namespace Skeleton.CleanArchitecture.Application;
public static class DependencyInjection
{

    /// <summary>
    /// Adds Mediatr.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void AddCustomMediatR(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Add MediatR - This adds all of the command and query handlers.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationMarker>());
    }
}

