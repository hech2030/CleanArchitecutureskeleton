using Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint;
using Skeleton.CleanArchitecture.Infrastructure.Helpers;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Skeleton.CleanArchitecture.Infrastructure.HttpClients.ExternalEndpoint
{
    public class ExternalEndpointHttpClientCacheDecorator : IExternalHttpClient
    {
        private readonly IExternalHttpClient _httpClient;
        private readonly IFusionCache _fusionCache;

        public ExternalEndpointHttpClientCacheDecorator(IExternalHttpClient httpClient, IFusionCache fusionCache)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(fusionCache);

            _httpClient = httpClient;
            _fusionCache = fusionCache;
        }


        public async Task<ExternalEndpointResponseMessage<Location>> GetLocation(string unlocationCode, CancellationToken cancellationToken)
        {
            var cachekey = CacheKeysHelper.GenerateKey<Location>(unlocationCode);
            var result = await _fusionCache.TryGetAsync<Location>(cachekey, token: cancellationToken);
            if (result.HasValue)
            {
                return new ExternalEndpointResponseMessage<Location> { Items = [result.Value] };
            }
            var httpResponse = await _httpClient.GetLocation(unlocationCode, cancellationToken);
            if (httpResponse is not null && httpResponse.Items is not null && httpResponse.Items.Any())
            {
                await _fusionCache.SetAsync(cachekey, httpResponse.Items.First(), _fusionCache.DefaultEntryOptions, cancellationToken);
                return httpResponse;
            }

            return new ExternalEndpointResponseMessage<Location>();
        }
    }
}
