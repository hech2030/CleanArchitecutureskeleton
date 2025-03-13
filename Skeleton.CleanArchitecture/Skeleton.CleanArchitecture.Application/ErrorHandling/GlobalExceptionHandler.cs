using Microsoft.AspNetCore.Http;
using Skeleton.CleanArchitecture.Application.Helpers.ISO8601;
using Skeleton.CleanArchitecture.Application.Helpers.Uris;
using Skeleton.CleanArchitecture.Domain.Entities.Common;
using System.Net.Mime;
using System.Text.Json;

namespace Skeleton.CleanArchitecture.Application.ErrorHandling;
public static class GlobalExceptionHandler
{
    public static readonly string DefaultErrorMessage = "Internal Server Error";
    public static readonly string DefaultVerboseErrorMessage = "We're sorry, but something went wrong on our end. Please try again later. If the issue persists, contact support for assistance.";

    public static async Task HandleHttpRequestAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var detailedError = new ErrorDetail
        {
            ErrorCodeText = DefaultErrorMessage,
            ErrorCodeMessage = DefaultVerboseErrorMessage
        };

        var errorResponse = new Error
        {
            ErrorDateTime = Iso8601FormatHelper.GetCurrentDateTimeIso8601Format(),
            Errors = [detailedError],
            HttpMethod = httpContext.Request.Method,
            ProviderCorrelationReference = Guid.NewGuid().ToString(),
            RequestUri = new Uri(InternalUriHelper.GetDisplayUrl(httpContext.Request)),
            StatusCode = httpContext.Response.StatusCode,
            StatusCodeText = DefaultErrorMessage,
        };
        var serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, serializerOptions);
    }
}
