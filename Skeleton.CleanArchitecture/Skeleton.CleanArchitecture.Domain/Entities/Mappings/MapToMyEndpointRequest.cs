using System.Diagnostics.CodeAnalysis;

namespace Msc.DF.CommercialSchedules.Domain.Entities.Mappings;
[ExcludeFromCodeCoverage]
public record MapMyEndpointRequest(
        string PlaceOfReceipt,
        string PlaceOfDelivery,
        bool PointOfOriginIsPort,
        bool PointOfFinalIsPort,
        int Limit)
{
}
