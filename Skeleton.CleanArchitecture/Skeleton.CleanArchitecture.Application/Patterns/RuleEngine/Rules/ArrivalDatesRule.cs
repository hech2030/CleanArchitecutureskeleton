using Microsoft.IdentityModel.Tokens;
using Skeleton.CleanArchitecture.Application.Common.Queries;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Interfaces;

namespace Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Rules;
public class ArrivalDatesRule : IDateMappingRule
{
    public bool IsMatch(MyEndpointQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);
        return !query.ArrivalStartDate.IsNullOrEmpty() || !query.ArrivalEndDate.IsNullOrEmpty();
    }


    public void Apply(MyEndpointQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        //Apply needed modifications here...
    }
}
