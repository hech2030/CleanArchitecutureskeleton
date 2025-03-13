using MediatR;
using Microsoft.Extensions.Logging;
using Msc.DF.CommercialSchedules.Application.Common.Queries;
using Skeleton.CleanArchitecture.Domain.Entities.MyEndpoint;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;

namespace Skeleton.CleanArchitecture.Application.Common.Queries;

public sealed class MyEndpointHandler : IRequestHandler<MyEndpointQuery, MyEndpointResponse>
{
    private readonly IMyEndpointService _myEndpointService;
    private readonly ILogger<MyEndpointHandler> _logger;

    public MyEndpointHandler(IMyEndpointService myEndpointService,
                               ILogger<MyEndpointHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(myEndpointService);
        ArgumentNullException.ThrowIfNull(logger);
        _myEndpointService = myEndpointService;
        _logger = logger;
    }
    public async Task<MyEndpointResponse> Handle(MyEndpointQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        //Check for cached data before processing

        // Check Ports
        _logger.LogInformation("Init check ports");


        var test = await _myEndpointService.GetLocationsFromUnlocation(string.Empty, cancellationToken);
        return new MyEndpointResponse();
    }
}

