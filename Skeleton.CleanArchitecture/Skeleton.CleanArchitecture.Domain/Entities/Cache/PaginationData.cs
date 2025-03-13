using Skeleton.CleanArchitecture.Domain.Entities.MyEndpoint;

namespace Skeleton.CleanArchitecture.Domain.Entities.Cache;
public record PaginationData(IEnumerable<Route>? Routes, int? Limit);