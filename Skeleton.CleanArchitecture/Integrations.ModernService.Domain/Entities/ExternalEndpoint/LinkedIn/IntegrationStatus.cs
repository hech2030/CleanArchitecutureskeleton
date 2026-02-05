namespace Integrations.ModernService.Domain.Entities.ExternalEndpoint.LinkedIn;

public class IntegrationStatus
{
    /// <summary>
    /// Data provider. (ie. "ATS")
    /// </summary>
    public string DataProvider { get; set; }

    /// <summary>
    /// Information about which org this integration is associated with.
    /// </summary>
    public string IntegrationContext { get; set; }

    /// <summary>
    /// Name of the integration.
    /// </summary>
    public string IntegrationName { get; set; }

    /// <summary>
    /// LinkedIn API code for this integration type.
    /// </summary>
    public string IntegrationType { get; set; }

    /// <summary>
    /// The status of the integration we're trying to enable.
    /// </summary>
    public string OnboardingStatus { get; set; }

    /// <summary>
    /// Tenant type.
    /// </summary>
    public string TenantType { get; set; }
}
