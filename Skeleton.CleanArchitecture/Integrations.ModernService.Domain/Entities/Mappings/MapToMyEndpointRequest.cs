using System.Diagnostics.CodeAnalysis;

namespace Integrations.ModernService.Domain.Entities.Mappings;
[ExcludeFromCodeCoverage]
public record MapMyEndpointRequest(
        string PlaceOfReceipt,
        string PlaceOfDelivery,
        bool PointOfOriginIsPort,
        bool PointOfFinalIsPort,
        int Limit)
{
}
