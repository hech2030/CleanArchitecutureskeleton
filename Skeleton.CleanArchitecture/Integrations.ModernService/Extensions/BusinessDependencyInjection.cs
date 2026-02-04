using Integrations.ModernService.Application.Services.TokenBuilder;
using Integrations.ModernService.DelegatingHandlers;
using Integrations.ModernService.Domain.Entities.Common.Options;
using Integrations.ModernService.Infrastructure.HttpClients.ExternalEndpoint;
using Integrations.ModernService.Infrastructure.HttpClients.Interfaces.Auth;
using Integrations.ModernService.Infrastructure.Interfaces;
using System.Net.Http.Headers;

namespace Integrations.ModernService.Extensions
{
    internal static class BusinessDependencyInjection
    {
        private const string ApplicationJsonMIMEType = "application/json";

        public static IServiceCollection AddBusinessInjection(
            this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.AddHttpClient();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<ExternalEndpointClientCredentialDelegatingHandler>();
            services.TryDecorate<IExternalHttpClient, ExternalEndpointHttpClientCacheDecorator>();
            return services;
        }

        private static void AddHttpClient(this IServiceCollection services)
        {
            const string client = nameof(client);

            services.AddHttpClient(client)
                .ConfigureHttpClient(CfgClient)
                .AddHttpMessageHandler<ExternalEndpointClientCredentialDelegatingHandler>()
                .AddTypedClient<IExternalHttpClient, ExternalEndpointHttpClient>()
                .AddStanderResilienceHandler();

            static void CfgClient(IServiceProvider provider, HttpClient client)
            {
                var config = provider.GetRequiredService<ExternalEndpointConfiguration>();
                client.BaseAddress = new Uri(config.ServiceBaseAddress);

                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJsonMIMEType));
            }
        }
    }
}
