using Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint;
using Location = Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint.Location;

namespace Skeleton.CleanArchitecture.Infrastructure.Interfaces;
public interface IExternalHttpClient
{
    Task<ExternalEndpointResponseMessage<Location>> GetLocation(string unlocationCode, CancellationToken cancellationToken);
}