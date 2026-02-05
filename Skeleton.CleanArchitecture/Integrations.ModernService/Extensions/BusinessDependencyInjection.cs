using Integrations.ModernService.Application.Services;
using Integrations.ModernService.DelegatingHandlers;
using Integrations.ModernService.Domain.Entities.Common.Options;
using Integrations.ModernService.Infrastructure.HttpClients.ExternalEndpoint;
using Integrations.ModernService.Infrastructure.HttpClients.TokenBuilders.LinkedIn;
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
            services.AddScoped<ILinkedInRetrieveTokenService, LinkedInRetrieveTokenService>();
            services.AddScoped<LinkedInEndpointClientCredentialDelegatingHandler>();
            services.AddScoped<ILinkedInIntegrationService, LinkedInIntegrationService>();
            services.AddScoped<ILinkedinRequisitionService, LinkedinRequisitionService>();

            ////services.TryDecorate<IExternalHttpClient, ExternalEndpointHttpClientCacheDecorator>();
            return services;
        }

        private static void AddHttpClient(this IServiceCollection services)
        {
            const string authClient = nameof(authClient);
            const string client = nameof(client);

            services.AddHttpClient(client)
                .ConfigureHttpClient(CfgClient)
                .AddHttpMessageHandler<LinkedInEndpointClientCredentialDelegatingHandler>()
                .AddTypedClient<ILinkedInEndpointHttpClient, LinkedInEndpointHttpClient>()
                .AddStanderResilienceHandler();
                
            services.AddHttpClient(authClient)
                .ConfigureHttpClient(CfgClient)
                .AddTypedClient<ILinkedInRetrieveTokenService, LinkedInRetrieveTokenService>();

            static void CfgClient(IServiceProvider provider, HttpClient client)
            {
                var config = provider.GetRequiredService<LinkedInServiceEndpointConfiguration>();
                client.BaseAddress = new Uri(config.ServiceBaseAddress);

                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJsonMIMEType));
            }
        }
    }
}
