using Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;

namespace Skeleton.CleanArchitecture.Application.Services
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
