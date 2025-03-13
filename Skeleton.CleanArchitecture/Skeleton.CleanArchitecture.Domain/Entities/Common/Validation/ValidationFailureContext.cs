using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Skeleton.CleanArchitecture.Domain.Entities.Common.Validation; 
public sealed class ValidationFailureContext
{
    public HttpContext HttpContext { get; }
    public IEnumerable<ValidationFailure> Errors { get; set; }
    public string ParameterName { get; }

    public ValidationFailureContext(HttpContext httpContext, IEnumerable<ValidationFailure> errors, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (string.IsNullOrWhiteSpace(parameterName))
        {
            throw new ArgumentException("Parameter name cannot be null or white space.", nameof(parameterName));
        }

        HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        Errors = errors.ToImmutableArray();
        ParameterName = parameterName;
    }

    public string GetHttpRequestDescription()
    {
        var method = HttpContext.Request.Method;
        var path = HttpContext.Request.Path;
        return $"{method.ToUpperInvariant()} {path}";
    }

    public override string ToString()
    {
        return $"{nameof(ValidationFailureContext)} for parameter {ParameterName} in HTTP request {GetHttpRequestDescription()}";
    }
}
