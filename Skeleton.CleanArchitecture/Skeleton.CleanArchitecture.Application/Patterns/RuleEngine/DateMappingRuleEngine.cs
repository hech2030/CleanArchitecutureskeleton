using Skeleton.CleanArchitecture.Application.Common.Queries;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Interfaces;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Rules;

namespace Skeleton.CleanArchitecture.Application.Patterns.RuleEngine;
public class DateMappingRuleEngine : IDateMappingRuleEngine
{
    private readonly List<IDateMappingRule> _rules =
    [
        new DepartureDatesRule(),
        new ArrivalDatesRule(),
        new DefaultDatesRule()
    ];

    public void MapDates(MyEndpointQuery query)
    {
        var rule = _rules.First(rule => rule.IsMatch(query));
        rule.Apply(query);
    }
}