using System.Diagnostics.CodeAnalysis;

namespace Skeleton.CleanArchitecture.Domain.Entities.Mappings;

[ExcludeFromCodeCoverage]
public record GetLegsMappingParams (bool PointOfOriginIsPort, bool PointOfFinalIsPort, string PlaceOfReceipt, string PlaceOfDelivery);
