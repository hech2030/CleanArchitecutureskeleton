﻿using Msc.DF.CommercialSchedules.Application.Common.Queries;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Interfaces;

namespace Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Rules;
public class DefaultDatesRule : IDateMappingRule
{
    public bool IsMatch(MyEndpointQuery query) => true;

    public void Apply(MyEndpointQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        //Apply needed modifications here...
    }
}

