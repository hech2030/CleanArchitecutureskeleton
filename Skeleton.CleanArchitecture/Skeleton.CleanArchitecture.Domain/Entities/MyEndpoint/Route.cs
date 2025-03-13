namespace Skeleton.CleanArchitecture.Domain.Entities.MyEndpoint; 
public class Route
{
    public string? ReceiptTypeAtOrigin { get; set; }
    public string? DeliveryTypeAtDestination { get; set; }
    public int SolutionNumber { get; set; }
    public int TransitTime { get; set; }
}