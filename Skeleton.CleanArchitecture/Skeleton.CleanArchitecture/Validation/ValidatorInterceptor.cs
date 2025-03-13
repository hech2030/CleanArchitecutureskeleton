using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Skeleton.CleanArchitecture.Application.Validation.Configuration;

namespace Skeleton.CleanArchitecture.Validation.Configuration;
/// <summary>
/// If invalid, add the ValidationResult to the HttpContext Items.
/// </summary>
internal class ValidatorInterceptor : IValidatorInterceptor
{
    private const string ValidationResult = nameof(ValidationResult);

    public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
    {
        ArgumentNullException.ThrowIfNull(actionContext);
        ArgumentNullException.ThrowIfNull(validationContext);
        ArgumentNullException.ThrowIfNull(result);

        var skipAttribute = actionContext.ActionDescriptor?.EndpointMetadata?.OfType<SkipValidationFilterAttribute>();
        if (skipAttribute?.Any() is true)
        {
            return new ValidationResult();
        }

        if (!result.IsValid)
        {
            actionContext.HttpContext?.Items?.Add(ValidationResult, result);
        }

        return result;
    }

    public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
    {
        ArgumentNullException.ThrowIfNull(actionContext);
        ArgumentNullException.ThrowIfNull(commonContext);

        return commonContext;
    }
}