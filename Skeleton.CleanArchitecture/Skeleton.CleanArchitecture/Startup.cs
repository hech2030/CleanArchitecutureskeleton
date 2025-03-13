using Asp.Versioning;
using Asp.Versioning.Conventions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Msc.DF.CommercialSchedules.Infrastructure.Interfaces.Validation;
using Serilog;
using Skeleton.CleanArchitecture.Application;
using Skeleton.CleanArchitecture.Application.ErrorHandling;
using Skeleton.CleanArchitecture.Application.Patterns.Factories;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Interfaces;
using Skeleton.CleanArchitecture.Application.Patterns.RuleEngine.Rules;
using Skeleton.CleanArchitecture.Application.Services;
using Skeleton.CleanArchitecture.Application.Validation;
using Skeleton.CleanArchitecture.Application.Validation.Configuration;
using Skeleton.CleanArchitecture.Authentication;
using Skeleton.CleanArchitecture.Extensions;
using Skeleton.CleanArchitecture.Health;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;
using Skeleton.CleanArchitecture.Validation;
using Skeleton.CleanArchitecture.Validation.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skeleton.CleanArchitecture;

public sealed class Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
{
    public void ConfigureServices(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);


        services.AddCustomOptions(configuration);
        services.AddAuthInjection(configuration);
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

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddMvc(options =>
        {
            options.Conventions.Add(new VersionByNamespaceConvention());
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
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

        //Rule Engine
        services.AddScoped<IDateMappingRule, DepartureDatesRule>();
        services.AddScoped<IDateMappingRule, ArrivalDatesRule>();
        services.AddScoped<IDateMappingRule, DefaultDatesRule>();

        //Register the Rule Engine
        services.AddScoped<IDateMappingRuleEngine, DateMappingRuleEngine>();

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

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/healthz");
        app.MapGet("/", () => string.Empty);
    }
}
