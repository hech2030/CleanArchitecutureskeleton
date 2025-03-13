using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Skeleton.CleanArchitecture;
using Skeleton.CleanArchitecture.Bootstrap;
using Skeleton.CleanArchitecture.Constants;
using Skeleton.CleanArchitecture.Extensions;

Log.Logger = LoggerHelpers.CreateBootstrapLogger();
Log.Information("Starting {ApplicationName}", ApiConstants.ApplicationName);
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Host.UseSerilog((context, serviceProvider, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(serviceProvider)
            .Enrich.WithExceptionDetails(
                new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
        );
    })
    .UseDatadog();

    var startup = new Startup(builder.Configuration, builder.Environment);
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();

    Startup.Configure(app);

    app.Run();

    Log.Information("{ApplicationName} stopped cleanly", ApiConstants.ApplicationName);
}
catch (Exception exception)
{
    Log.Fatal(exception, "{ApplicationName} terminated unexpectedly", ApiConstants.ApplicationName);
}
finally
{
    Log.CloseAndFlush();
}
