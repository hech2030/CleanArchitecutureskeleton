using Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint;

namespace Skeleton.CleanArchitecture.Infrastructure.Interfaces;
public interface IMyEndpointService
{
    Task<ExternalEndpointResponseMessage<Location>> GetLocationsFromUnlocation(string UnlocationCode, CancellationToken cancellationToken);
}