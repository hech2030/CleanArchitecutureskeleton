using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;
using Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint;
using Skeleton.CleanArchitecture.Infrastructure.HttpClients.Extensions;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;

namespace Skeleton.CleanArchitecture.Infrastructure.HttpClients.ExternalEndpoint;
public class ExternalEndpointHttpClient : IExternalHttpClient
{
    protected readonly ILogger<ExternalEndpointHttpClient> _logger;
    protected readonly HttpClient _httpClient;
    protected readonly ExternalEndpointConfiguration _externalEndpointConfiguration;
    public ExternalEndpointHttpClient(
       HttpClient httpClient,
       IOptions<ExternalEndpointConfiguration> externalEndpointConfiguration,
       ILogger<ExternalEndpointHttpClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(externalEndpointConfiguration?.Value);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _externalEndpointConfiguration = externalEndpointConfiguration!.Value;
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

    Task<ExternalEndpointResponseMessage<Location>> IExternalHttpClient.GetLocation(string unlocationCode, CancellationToken cancellationToken)
    {
        return GetResponse<ExternalEndpointResponseMessage<Location>>(new Dictionary<string, string>
        {
            ["UnLocationCode"] = unlocationCode
        }, _externalEndpointConfiguration.Endpoints.Location!, cancellationToken);
    }
}
