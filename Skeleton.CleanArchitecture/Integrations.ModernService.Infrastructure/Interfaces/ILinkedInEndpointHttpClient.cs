using Integrations.ModernService.Domain.Entities.ExternalEndpoint;

namespace Integrations.ModernService.Infrastructure.Interfaces;
public interface ILinkedInEndpointHttpClient
{
    Task<ExternalEndpointResponseMessage<Location>> GetLocation(string unlocationCode, CancellationToken cancellationToken);
    Task<string> GetIntegrationStatus(string integrationContext, string type, string tenant, CancellationToken cancellationToken);
}