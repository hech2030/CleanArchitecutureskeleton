using Integrations.ModernService.Domain.Entities.Common.Options;
using Integrations.ModernService.Infrastructure.HttpClients.Extensions;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;
using System;

namespace Integrations.ModernService.Infrastructure.HttpClients.TokenBuilders.LinkedIn
{
    public class LinkedInRetrieveTokenService : ILinkedInRetrieveTokenService
    {
        protected readonly HttpClient _httpClient;
        protected readonly LinkedInServiceEndpointConfiguration _LinkedInEndpointConfiguration;
        protected readonly IFusionCache _cache;
        private const string CacheKey = "linkedin_token";

        public LinkedInRetrieveTokenService(
            HttpClient httpClient,
            IOptions<LinkedInServiceEndpointConfiguration> LinkedInEndpointConfiguration,
            IFusionCache cache)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(LinkedInEndpointConfiguration?.Value);
            ArgumentNullException.ThrowIfNull(cache);

            _LinkedInEndpointConfiguration = LinkedInEndpointConfiguration!.Value;
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<LinkedInTokenModel> RetrieveToken(CancellationToken cancellationToken)
        {
            // Try to get the token from cache
            var cachedToken = await _cache.GetOrDefaultAsync<LinkedInTokenModel>(CacheKey, token: cancellationToken);

            if (cachedToken != null)
            {
                return cachedToken;
            }

            // Token is missing or expired, fetch a new one
            var parameters = new Dictionary<string, string>
            {
                { "clientId", _LinkedInEndpointConfiguration.Secrets.ClientId },
                { "clientSecret", _LinkedInEndpointConfiguration.Secrets.ClientSecret },
                { "grantType", "client_credentials" }
            };
            using var requestMessage = RestClientExtension.PrepareEnhancedRequestMessage(HttpMethod.Get, _LinkedInEndpointConfiguration.ServiceBaseAddress, parameters);
            var newToken = await _httpClient.SendAndReadAsAsync(requestMessage, OnDeserializationErrorAsync<LinkedInTokenModel>, cancellationToken);

            if (newToken != null && newToken.ExpiresIn > 0)
            {
                var duration = TimeSpan.FromSeconds(newToken.ExpiresIn);
                await _cache.SetAsync(CacheKey, newToken, duration, token: cancellationToken);
            }

            return newToken!;
        }

        private Task<T> OnDeserializationErrorAsync<T>(HttpResponseMessage resultMessage)
        {
            return Task.FromResult(default(T)!);
        }
    }
}
