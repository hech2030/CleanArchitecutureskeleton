namespace Skeleton.CleanArchitecture.Bootstrap;

public static class ConfigurationHelpers
{
    private const string ConfigurationFileName = "appsettings.json";
    private const string AspNetCoreEnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";

    private const bool IS_MANDATORY_FILE = false;
    private const bool DO_NOT_RELOAD_ON_CHANGE = false;
    private const bool IS_OPTIONAL_FILE = true;

    public static IConfigurationRoot CreateBootstrapConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(
            ConfigurationFileName,
            IS_MANDATORY_FILE,
            DO_NOT_RELOAD_ON_CHANGE);

        var environmentName = GetEnvironmentName();
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            configurationBuilder.AddJsonFile(
                GetEnvironmentSpecificConfigurationFileName(environmentName),
                IS_OPTIONAL_FILE,
                DO_NOT_RELOAD_ON_CHANGE
            );
        }

        return configurationBuilder.Build();

        static string? GetEnvironmentName() =>
            Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableName);

        static string GetEnvironmentSpecificConfigurationFileName(string environmentName) =>
            $"appsettings.{environmentName}.json";
    }
}
