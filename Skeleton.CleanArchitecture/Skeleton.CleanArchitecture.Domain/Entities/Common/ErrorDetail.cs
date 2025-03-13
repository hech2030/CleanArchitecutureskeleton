namespace Skeleton.CleanArchitecture.Domain.Entities.Common;

public class ErrorDetail
{
    public string? ErrorCode { get; set; }
    public string? Property { get; set; }
    public string? Value { get; set; }
    public string? JsonPath { get; set; }
    public string? ErrorCodeText { get; set; }
    public string? ErrorCodeMessage { get; set; }
}
