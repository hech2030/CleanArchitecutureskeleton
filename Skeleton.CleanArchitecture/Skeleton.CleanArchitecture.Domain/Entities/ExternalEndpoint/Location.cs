using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Skeleton.CleanArchitecture.Domain.Entities.ExternalEndpoint
{
    [ExcludeFromCodeCoverage]
    public class Location
    {
        [JsonPropertyName("LocationId")]
        public string LocationId { get; set; } = string.Empty;

        [JsonPropertyName("Latitude")]
        public string? Latitude { get; set; }

        [JsonPropertyName("Longitude")]
        public string? Longitude { get; set; }

        [JsonPropertyName("Comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("CountryId")]
        public int? CountryId { get; set; }

        [JsonPropertyName("CountrySubdivisionId")]
        public int? CountrySubdivisionId { get; set; }

        [JsonPropertyName("Level")]
        public int? Level { get; set; }

        [JsonPropertyName("TimeZoneId")]
        public int? TimeZoneId { get; set; }
    }
}
