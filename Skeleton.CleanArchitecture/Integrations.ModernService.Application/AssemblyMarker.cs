using System.Reflection;

namespace Integrations.ModernService.Application;

public sealed class ApplicationMarker
{
    private ApplicationMarker()
    {
    }

    public static Assembly Assembly { get; } = typeof(ApplicationMarker).Assembly;
}
