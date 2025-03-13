using System.Diagnostics.CodeAnalysis;

namespace Skeleton.CleanArchitecture.Domain.Entities.Common.Options;
[ExcludeFromCodeCoverage]
public class ExternalEndpointConfiguration
{
    /// <summary>
    /// Gets or sets the Service Base Address.
    /// </summary>
    /// <value>
    /// The Service Base Address.
    /// </value>
    public string ServiceBaseAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the scope.
    /// </summary>
    /// <value>
    /// The scope.
    /// </value>
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the endpoints list.
    /// </summary>
    public required ExternalEndpoints Endpoints { get; set; }
}


