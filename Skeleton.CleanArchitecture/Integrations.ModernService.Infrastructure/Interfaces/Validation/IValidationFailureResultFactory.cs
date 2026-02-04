using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Integrations.ModernService.Infrastructure.Interfaces.Validation;
public interface IValidationFailureResultFactory
{
    Task<IActionResult> CreateResultAsync(HttpContext context, ValidationResult validationResult, CancellationToken cancellationToken);
}
