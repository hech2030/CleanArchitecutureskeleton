using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Skeleton.CleanArchitecture.Application.Helpers.ISO8601;
using Skeleton.CleanArchitecture.Application.Helpers.Uris;
using Skeleton.CleanArchitecture.Domain.Entities.Common;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Validation;
using System.Diagnostics;
using System.Net;

namespace Skeleton.CleanArchitecture.Application.Patterns.Factories;

public static class ErrorFactory
{
    private const string InvalidData = nameof(InvalidData);
    private const string BadRequestCode = "400";
    public static readonly string VALIDATION_ERR_MSG = "The supplied data could not be accepted";

    internal static ValidationError CreateValidationError(
        HttpRequest request,
        IReadOnlyCollection<FluentValidation.Results.ValidationFailure> errors)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(errors);

        var statusCode = HttpStatusCode.BadRequest;

        var result = BaseValidationError(request, statusCode);

        if (errors.Count > 0)
        {
            var validationErrors = errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(vf => vf).ToArray());

            foreach (var validationError in validationErrors)
            {
                foreach (var validationFailure in validationError.Value)
                {
                    var validatorState = validationFailure.CustomState as ValidatorState ?? new ValidatorState(string.Empty);
                    var error = new ErrorDetail
                    {
                        ErrorCode = !string.IsNullOrEmpty(validatorState.ErrorCode) ? validatorState.ErrorCode : validationFailure.ErrorCode,
                        Property = validationFailure.PropertyName,
                        Value = validationFailure.AttemptedValue?.ToString(),
                        ErrorCodeText = InvalidData,
                        ErrorCodeMessage = validationFailure.ErrorMessage,
                    };
                    result.Errors?.Add(error);
                }
            }
        }

        return result;
    }

    internal static ValidationError CreateValidationError(
        HttpRequest request, string property, string? value)
    {
        ArgumentNullException.ThrowIfNull(request);

        var statusCode = HttpStatusCode.BadRequest;

        var result = BaseValidationError(request, statusCode);

        var error = new ErrorDetail
        {
            ErrorCode = BadRequestCode,
            Property = property,
            Value = value,
            ErrorCodeText = InvalidData,
            ErrorCodeMessage = $"Invalid value {value} for property {property}",
        };
        result.Errors?.Add(error);


        return result;
    }

    private static ValidationError BaseValidationError(
        HttpRequest request,
        HttpStatusCode statusCode)
    {
        var baseError = new ValidationError(BaseError(request, statusCode))
        {
            StatusCodeText = VALIDATION_ERR_MSG
        };
        return baseError;
    }

    private static Error BaseError(HttpRequest request, HttpStatusCode statusCode)
    {
        var operationId = Activity.Current?.RootId;
        var uri = InternalUriHelper.GetDisplayUrl(request);
        var reason = ReasonPhrases.GetReasonPhrase((int)statusCode);

        var result = new Error
        {
            ErrorDateTime = Iso8601FormatHelper.GetCurrentDateTimeIso8601Format(),
            Errors = [],
            HttpMethod = request.Method,
            ProviderCorrelationReference = operationId,
            RequestUri = new Uri(uri),
            StatusCode = (int)statusCode,
            StatusCodeText = reason,
        };
        return result;
    }

    internal static Task<IActionResult> GetActionResult(Error validationError)
    {
        var httpStatusCode = (HttpStatusCode)validationError.StatusCode!;
        return httpStatusCode switch
        {
            HttpStatusCode.Conflict => Task.FromResult<IActionResult>(new ConflictObjectResult(validationError)),
            HttpStatusCode.BadRequest => Task.FromResult<IActionResult>(new BadRequestObjectResult(validationError)),
            HttpStatusCode.NotFound => Task.FromResult<IActionResult>(new NotFoundObjectResult(validationError)),
            _ => Task.FromResult<IActionResult>(new BadRequestObjectResult(validationError)),
        };
    }
    internal static void LogFailure(
        HttpContext? httpContext,
        string failureType,
        Dictionary<string, string> validationFailures,
        ILogger logger)
    {
        TryAddFailureSharedProperties(httpContext, validationFailures);

        var operationId = Activity.Current?.RootId;
        logger.LogError(
            "{FailureType} Validation failed for {OperationId}. Error: {@Error}",
            failureType,
            operationId,
            validationFailures);
    }

    // Common helper for adding shared properties from HttpContext
    private static void TryAddFailureSharedProperties(
        HttpContext? context,
        Dictionary<string, string> errors)
    {
        if (context?.Request?.Path != null)
        {
            // Using TryAdd to ensure we don't overwrite an existing key.
            errors.TryAdd("EndpointRoute", context.Request.Path);
        }
    }
}
