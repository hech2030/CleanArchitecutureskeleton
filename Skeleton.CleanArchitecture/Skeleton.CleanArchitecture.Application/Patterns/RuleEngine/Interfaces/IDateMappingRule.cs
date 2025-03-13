using Skeleton.CleanArchitecture.Application.Common.Queries;

namespace Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Interfaces;
public interface IDateMappingRule
{
    bool IsMatch(MyEndpointQuery query);
    void Apply(MyEndpointQuery query);
}

