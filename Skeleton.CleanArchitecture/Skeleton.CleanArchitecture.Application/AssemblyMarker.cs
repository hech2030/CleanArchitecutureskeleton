using System.Reflection;

namespace Skeleton.CleanArchitecture.Application;

public sealed class ApplicationMarker
{
    private ApplicationMarker()
    {
    }

    public static Assembly Assembly { get; } = typeof(ApplicationMarker).Assembly;
}
