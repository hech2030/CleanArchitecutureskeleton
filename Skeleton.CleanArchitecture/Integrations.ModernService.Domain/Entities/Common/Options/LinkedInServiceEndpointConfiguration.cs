using System.Diagnostics.CodeAnalysis;

namespace Integrations.ModernService.Domain.Entities.Common.Options;
[ExcludeFromCodeCoverage]
public class LinkedInServiceEndpointConfiguration
{
    /// <summary>
    /// Gets or sets the Service Base Address.
    /// </summary>
    /// <value>
    /// The Service Base Address.
    /// </value>
    public string ServiceBaseAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the endpoints list.
    /// </summary>
    public required LinkedInEndpointConfiguration Endpoints { get; set; }

    public required LinkedInSecrets Secrets { get; set; }
}


