using Integrations.ModernService.Domain.Entities.ExternalEndpoint;
using Location = Integrations.ModernService.Domain.Entities.ExternalEndpoint.Location;

namespace Integrations.ModernService.Infrastructure.Interfaces;
public interface IExternalHttpClient
{
    Task<ExternalEndpointResponseMessage<Location>> GetLocation(string unlocationCode, CancellationToken cancellationToken);
}