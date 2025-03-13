namespace Skeleton.CleanArchitecture.Domain.Entities.Common;

public class Error
{
    public string? HttpMethod { get; set; }
    public Uri? RequestUri { get; set; }
    public int StatusCode { get; set; }
    public string? StatusCodeText { get; set; }
    public string? ProviderCorrelationReference { get; set; }
    public string? ErrorDateTime { get; set; }
    public IList<ErrorDetail>? Errors { get; set; }
}
