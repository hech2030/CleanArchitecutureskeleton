namespace Skeleton.CleanArchitecture.Domain.Entities.MyEndpoint;
public class MyEndpointResponse
{
    public IEnumerable<Route> Routes { get; set; } = [];

    public string? Cursor { get; set; }
}
