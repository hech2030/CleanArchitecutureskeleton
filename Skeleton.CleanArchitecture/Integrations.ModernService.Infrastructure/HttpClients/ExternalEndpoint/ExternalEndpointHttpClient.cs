using Integrations.ModernService.Domain.Entities.Common.Options;
using Integrations.ModernService.Domain.Entities.ExternalEndpoint;
using Integrations.ModernService.Infrastructure.HttpClients.Extensions;
using Integrations.ModernService.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Integrations.ModernService.Domain.Entities.ExternalEndpoint.LinkedIn;

namespace Integrations.ModernService.Infrastructure.HttpClients.ExternalEndpoint;

public class LinkedInEndpointHttpClient : ILinkedInEndpointHttpClient
{
    protected readonly ILogger<LinkedInEndpointHttpClient> _logger;
    protected readonly HttpClient _httpClient;
    protected readonly LinkedInServiceEndpointConfiguration _LinkedInEndpointConfiguration;
    public LinkedInEndpointHttpClient(
       HttpClient httpClient,
       IOptions<LinkedInServiceEndpointConfiguration> LinkedInEndpointConfiguration,
       ILogger<LinkedInEndpointHttpClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(LinkedInEndpointConfiguration?.Value);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _LinkedInEndpointConfiguration = LinkedInEndpointConfiguration!.Value;
        _logger = logger;
    }

    private Task<T> GetResponse<T>(
        IDictionary<string, string> parameters,
        string externalEnpoint, CancellationToken
        cancellationToken) where T : class, new()
    {
        using var requestMessage = RestClientExtension.PrepareEnhancedRequestMessage(HttpMethod.Get, externalEnpoint, parameters);
        return _httpClient.SendAndReadAsAsync(requestMessage, OnDeserializationErrorAsync<T>, cancellationToken);
    }

    private Task<T> OnDeserializationErrorAsync<T>(HttpResponseMessage resultMessage)
    {
        _logger.LogError("Recieved {StatusCode} from external endpoint", resultMessage.StatusCode);
        return Task.FromResult(default(T)!);
    }

    Task<ExternalEndpointResponseMessage<Location>> ILinkedInEndpointHttpClient.GetLocation(string unlocationCode, CancellationToken cancellationToken)
    {
        return GetResponse<ExternalEndpointResponseMessage<Location>>(new Dictionary<string, string>
        {
            ["UnLocationCode"] = unlocationCode
        }, _LinkedInEndpointConfiguration.Endpoints.JobRequisitions!, cancellationToken);
    }

    public async Task<string> GetIntegrationStatus(string integrationContext, string type, string tenant, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            ["integrationContextValue"] = integrationContext,
            ["integrationTypeValue"] = type,
            ["tenantTypeValue"] = tenant
        };
        var response = await GetResponse<GetStatusResponse<IntegrationStatus>>(parameters,
            _LinkedInEndpointConfiguration.Endpoints.IntegrationEnabled!,
            cancellationToken);

        if (response is not null && response.Results.Any())
        {
            return response.Results.First().Value.OnboardingStatus;
        }
        return string.Empty;
    }
}
