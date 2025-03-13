using Serilog;

namespace Skeleton.CleanArchitecture.Bootstrap;

public static class LoggerHelpers
{
    public static Serilog.ILogger CreateBootstrapLogger()
    {
        var configuration = ConfigurationHelpers.CreateBootstrapConfiguration();

        return Factory.Create<LoggerConfiguration>()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
    }
}
