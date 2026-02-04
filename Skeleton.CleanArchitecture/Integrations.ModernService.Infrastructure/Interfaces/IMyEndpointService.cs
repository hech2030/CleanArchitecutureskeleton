using Integrations.ModernService.Domain.Entities.ExternalEndpoint;

namespace Integrations.ModernService.Infrastructure.Interfaces;
public interface IMyEndpointService
{
    Task<ExternalEndpointResponseMessage<Location>> GetLocationsFromUnlocation(string UnlocationCode, CancellationToken cancellationToken);
}