using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint
{
    [ExcludeFromCodeCoverage]
    public class ExternalEndpointResponseMessage<T>
    {
        [JsonPropertyName("Items")]
        public IEnumerable<T>? Items { get; set; }
    }
}
