using System.Diagnostics.CodeAnalysis;

namespace Msc.DF.CommercialSchedules.Domain.Entities.Mappings;

[ExcludeFromCodeCoverage]
public record GetLegsMappingParams (bool PointOfOriginIsPort, bool PointOfFinalIsPort, string PlaceOfReceipt, string PlaceOfDelivery);
