using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Integrations.ModernService.Domain.Entities.ExternalEndpoint
{
    [ExcludeFromCodeCoverage]
    public class ExternalEndpointResponseMessage<T>
    {
        [JsonPropertyName("Items")]
        public IEnumerable<T>? Items { get; set; }
    }
}
