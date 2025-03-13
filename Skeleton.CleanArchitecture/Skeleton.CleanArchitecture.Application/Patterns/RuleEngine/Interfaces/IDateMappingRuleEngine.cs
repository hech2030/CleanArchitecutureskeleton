using Skeleton.CleanArchitecture.Application.Common.Queries;

namespace Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Interfaces;
public interface IDateMappingRuleEngine
{
    void MapDates(MyEndpointQuery query);
}
