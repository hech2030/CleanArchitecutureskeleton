using System.Diagnostics.CodeAnalysis;

namespace Integrations.ModernService.Domain.Entities.Mappings;

[ExcludeFromCodeCoverage]
public record GetLegsMappingParams (bool PointOfOriginIsPort, bool PointOfFinalIsPort, string PlaceOfReceipt, string PlaceOfDelivery);
