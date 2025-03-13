using Datadog.Trace;
using Datadog.Trace.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;

namespace Skeleton.CleanArchitecture.Extensions;
public static class DatadogHostBuilderExtensions
{
    public static IHostBuilder UseDatadog(this IHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.UseDatadog(nameof(DatadogOptions), (ctx, setting) => { });
    }

    internal static IHostBuilder UseDatadog(
    this IHostBuilder builder,
    string datadogSectionName,
    Action<HostBuilderContext, TracerSettings> configureDelegate)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureServices(delegate (HostBuilderContext ctx, IServiceCollection collection)
        {

            var datadogOptions = ctx.Configuration.GetSection(datadogSectionName).Get<DatadogOptions>();

            ArgumentNullException.ThrowIfNull(datadogOptions);

            var settings = TracerSettings.FromDefaultSources();
            settings.TraceEnabled = datadogOptions.Enabled;
            settings.Environment = datadogOptions.Environment;
            settings.ServiceVersion = datadogOptions.Version;
            settings.ServiceName = datadogOptions.Service;
            settings.AddToGlobalTags(new Dictionary<string, string>
            {
                ["project_code"] = datadogOptions.ProjectCode,
                ["dd.project_code"] = datadogOptions.ProjectCode,
            });

            settings.SetHttpServerErrorStatusCodes(HttpServerErrorStatusCodes);

            // override if available.
            configureDelegate?.Invoke(ctx, settings);

            settings.TracerMetricsEnabled = true;
            settings.LogsInjectionEnabled = true;


            Tracer.Configure(settings);

            collection.Remove<TracerSettings>();
            collection.TryAddSingleton(settings);
        });

        return builder;
    }

    private static readonly int[] HttpServerErrorStatusCodes = [.. Enumerable.Range(400, 599)];

    /// <summary>
    /// (settings.GlobalTags ??= new Dictionarystring, string()).Add("", "");
    /// settings.GlobalTags ?? (settings.GlobalTags = new Dictionarystring string()).Add("", "");
    /// </summary>
    /// <param name="tracerSettings"></param>
    /// <param name="tagsToAdd"></param>
    private static void AddToGlobalTags(this TracerSettings tracerSettings, Dictionary<string, string> tagsToAdd)
    {
        if (tagsToAdd == null || tagsToAdd.Count <= 0)
        {
            return;
        }

        tracerSettings.GlobalTags ??= new Dictionary<string, string>();
        foreach (var tag in tagsToAdd)
        {
            tracerSettings.GlobalTags[tag.Key] = tag.Value;
        }
    }
}
