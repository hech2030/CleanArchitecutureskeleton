using Integrations.ModernService.Domain.Entities.ExternalEndpoint;
using Integrations.ModernService.Infrastructure.Interfaces;

namespace Integrations.ModernService.Application.Services
{
    public class MyEndpointService : IMyEndpointService
    {
        private readonly IExternalHttpClient _externalHttpClient;
        public MyEndpointService(IExternalHttpClient externalHttpClient)
        {
            ArgumentNullException.ThrowIfNull(externalHttpClient);
            _externalHttpClient = externalHttpClient;
        }


        public Task<ExternalEndpointResponseMessage<Location>> GetLocationsFromUnlocation(string UnlocationCode, CancellationToken cancellationToken)
        {
            return _externalHttpClient.GetLocation(UnlocationCode, cancellationToken);
        }
    }
}
