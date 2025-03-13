namespace Skeleton.CleanArchitecture.Domain.Entities.Common;

public class ValidationError : Error
{
    public ValidationError(Error commonError)
    {
        ArgumentNullException.ThrowIfNull(commonError);

        HttpMethod = commonError.HttpMethod;
        RequestUri = commonError.RequestUri;
        Errors = commonError.Errors;
        StatusCode = commonError.StatusCode;
        StatusCodeText = commonError.StatusCodeText;
        ErrorDateTime = commonError.ErrorDateTime;
        ProviderCorrelationReference = commonError.ProviderCorrelationReference;
    }
    public ValidationError(IList<ErrorDetail> subErrors)
        : this()
    {
        Errors = subErrors;
    }

    public ValidationError()
    {
    }
}
