using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;
using Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint;
using Skeleton.CleanArchitecture.Infrastructure.HttpClients.Extensions;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;

namespace Msc.DF.CommercialSchedules.Infrastructure.HttpClients.Mdm;
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

    private Task<T> GetMdmResponse<T>(
        IDictionary<string, string> parameters,
        string mdmEnpoint, CancellationToken
        cancellationToken) where T : class, new()
    {
        using var requestMessage = RestClientExtension.PrepareEnhancedRequestMessage(HttpMethod.Get, mdmEnpoint, parameters);
        return _httpClient.SendAndReadAsAsync(requestMessage, OnDeserializationMdmErrorAsync<T>, cancellationToken);
    }

    private Task<T> OnDeserializationMdmErrorAsync<T>(HttpResponseMessage resultMessage)
    {
        _logger.LogError("Recieved {StatusCode} from MDM", resultMessage.StatusCode);
        return Task.FromResult(default(T)!);
    }

    Task<ExternalEndpointResponseMessage<Location>> IExternalHttpClient.GetLocation(string unlocationCode, CancellationToken cancellationToken)
    {
        return GetMdmResponse<ExternalEndpointResponseMessage<Location>>(new Dictionary<string, string>
        {
            ["UnLocationCode"] = unlocationCode
        }, _externalEndpointConfiguration.Endpoints.Location!, cancellationToken);
    }
}
