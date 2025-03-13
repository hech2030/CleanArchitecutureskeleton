namespace Skeleton.CleanArchitecture.Infrastructure.Helpers;
public static class CacheKeysHelper
{
    public static string GenerateKey<T>(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return $"{typeof(T).Name}:{value}";
    }
    public static string GenerateKey(string prefix, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return $"{prefix}:{value}";
    }
}