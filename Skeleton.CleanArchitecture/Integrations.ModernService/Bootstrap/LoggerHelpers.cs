using Serilog;

namespace Integrations.ModernService.Bootstrap;

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
