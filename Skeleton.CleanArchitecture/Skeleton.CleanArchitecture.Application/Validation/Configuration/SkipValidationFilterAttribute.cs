using System.Diagnostics.CodeAnalysis;

namespace Skeleton.CleanArchitecture.Application.Validation.Configuration;
/// <summary>
/// Skip Validation model over endpoint.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method)]
public class SkipValidationFilterAttribute : Attribute
{
}
