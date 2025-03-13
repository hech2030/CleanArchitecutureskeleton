using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Skeleton.CleanArchitecture.Application.Patterns.Factories;

namespace Skeleton.CleanArchitecture.Application.Validation.Configuration;
public class ValidationFilterAttribute : ActionFilterAttribute
{
    private const string ValidationResult = nameof(ValidationResult);
    private const string Business = nameof(Business);
    private readonly ILogger<ValidationFilterAttribute> _logger;

    public ValidationFilterAttribute(
        ILogger<ValidationFilterAttribute> injectedLogger)
    {
        ArgumentNullException.ThrowIfNull(injectedLogger);
        _logger = injectedLogger;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            if (!context.ModelState.IsValid)
            {
                await OnModelStateFailure(context);
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "object not found");
            return;
        }

        await next();
    }

    private async Task OnModelStateFailure(ActionExecutingContext context)
    {
        ErrorFactory.LogFailure(context.HttpContext, "Model State", [], _logger);
        context.Result = await CreateModelStateError(context);
    }

    private static Task<IActionResult> CreateModelStateError(ActionExecutingContext context)
    {
        var httpRequest = context.HttpContext.Request;
        var errors = context.ModelState.FirstOrDefault(x => x.Value?.ValidationState == ModelValidationState.Invalid);
        var validationError = ErrorFactory.CreateValidationError(
            httpRequest,
            errors.Key,
            errors.Value?.AttemptedValue);

        return ErrorFactory.GetActionResult(validationError);

    }
}
