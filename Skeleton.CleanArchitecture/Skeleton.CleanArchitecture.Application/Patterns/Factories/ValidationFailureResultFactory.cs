using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msc.DF.CommercialSchedules.Infrastructure.Interfaces.Validation;

namespace Skeleton.CleanArchitecture.Application.Patterns.Factories;

public class ValidationFailureResultFactory : IValidationFailureResultFactory
{
    private const string ValidationResult = nameof(ValidationResult);
    private const string Business = nameof(Business);

    private readonly ILogger<ValidationFailureResultFactory> _logger;

    public ValidationFailureResultFactory(ILogger<ValidationFailureResultFactory> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public async Task<IActionResult> CreateResultAsync(HttpContext context, ValidationResult validationResult, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        ErrorFactory.LogFailure(context, Business, [], _logger);
        return await CreateValidationError(context, validationResult);

    }
    private static Task<IActionResult> CreateValidationError(HttpContext context, ValidationResult validationResult)
    {
        var httpRequest = context.Request;
        var errors = validationResult.Errors;
        var validationError = ErrorFactory.CreateValidationError(
            httpRequest,
            errors);

        return ErrorFactory.GetActionResult(validationError);
    }
}
