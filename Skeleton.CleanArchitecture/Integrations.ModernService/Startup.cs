using FluentValidation;
using FluentValidation.AspNetCore;
using Integrations.ModernService.Application;
using Integrations.ModernService.Application.ErrorHandling;
using Integrations.ModernService.Application.Patterns.Factories;
using Integrations.ModernService.Application.Services;
using Integrations.ModernService.Application.Validation;
using Integrations.ModernService.Application.Validation.Configuration;
using Integrations.ModernService.Extensions;
using Integrations.ModernService.Health;
using Integrations.ModernService.Infrastructure.Interfaces;
using Integrations.ModernService.Infrastructure.Interfaces.Validation;
using Integrations.ModernService.Validation;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Integrations.ModernService;

public sealed class Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
{
    public void ConfigureServices(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCustomOptions(configuration);
        services.AddCustomMediatR();

        services.AddCacheSystemInjection(configuration, hostEnvironment);

        services
            .AddControllers(options =>
            {
                options.Filters.Add<ValidationFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            })
            .ConfigureApiBehaviorOptions((options) =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

        // Configure FluentValidation
        ValidatorOptions.Global.DisplayNameResolver = (type, member, expr) => member?.Name;
        services
            .AddValidatorsFromAssemblyContaining<ValidationMarker>()
            .AddValidatorsFromAssemblyContaining<ApiValidationMarker>()
            .AddScoped<IValidatorInterceptor, ValidatorInterceptor>()
            .AddTransient<IValidationFailureResultFactory, ValidationFailureResultFactory>();
        services.AddBusinessInjection();
        services.AddScoped<IMyEndpointService, MyEndpointService>();

        // Configure health checks.
        services.AddHealthCheckServices(configuration);
    }

    public static void Configure(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseForwardedHeaders();

        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(GlobalExceptionHandler.HandleHttpRequestAsync);
        });

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("client-IP", httpContext.Connection.RemoteIpAddress!);
                diagnosticContext.Set("User-Agent", httpContext.Request.Headers.UserAgent.ToString());
            };
        });

        app.MapControllers();
        app.MapHealthChecks("/healthz");
    }
}
