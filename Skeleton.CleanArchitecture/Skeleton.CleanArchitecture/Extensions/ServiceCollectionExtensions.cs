using System.Data;

namespace Skeleton.CleanArchitecture.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            if (services.IsReadOnly)
            {
                throw new ReadOnlyException($"{nameof(services)} is read only");
            }

            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }
            return services;
        }

        internal static IServiceCollection AddIf(this IServiceCollection services,
           bool condition,
           Func<IServiceCollection, IServiceCollection> action)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(action);

            if (condition)
            {
                services = action(services);
            }

            return services;
        }

    }
}
