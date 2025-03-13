namespace Skeleton.CleanArchitecture.Bootstrap;

public static class Factory
{
    public static T Create<T>() where T : new() =>
        new();
}

