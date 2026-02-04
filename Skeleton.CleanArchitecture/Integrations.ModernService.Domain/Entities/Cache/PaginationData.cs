using Integrations.ModernService.Domain.Entities.MyEndpoint;

namespace Integrations.ModernService.Domain.Entities.Cache;
public record PaginationData(IEnumerable<Route>? Routes, int? Limit);