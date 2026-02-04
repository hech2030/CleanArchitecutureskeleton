using MediatR;
using Integrations.ModernService.Domain.Entities.MyEndpoint;

namespace Integrations.ModernService.Application.Common.Queries;
public sealed class MyEndpointQuery : IRequest<MyEndpointResponse>
{
    public string PlaceOfReceipt { get; set; } = string.Empty;
    public string PlaceOfDelivery { get; set; } = string.Empty;
    public string? DepartureStartDate { get; set; }
    public string? DepartureEndDate { get; set; }
    public string? ArrivalStartDate { get; set; }
    public string? ArrivalEndDate { get; set; }
    public int? MaxTranshipment { get; set; }
    public string? ReceiptTypeAtOrigin { get; set; } = string.Empty;
    public string? DeliveryTypeAtDestination { get; set; } = string.Empty;
    public int? Limit { get; set; } = 100;
    public string? Cursor { get; set; } = string.Empty;
}