using Integrations.ModernService.Domain.Entities.ExternalEndpoint;

namespace Integrations.ModernService.Infrastructure.Interfaces;
public interface ILinkedInIntegrationService
{
    Task<bool> IsIntegrationEnabled(int orgId, CancellationToken cancellationToken);
}