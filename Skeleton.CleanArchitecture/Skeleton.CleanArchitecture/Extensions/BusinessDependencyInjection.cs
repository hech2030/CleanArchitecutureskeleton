using Msc.DF.CommercialSchedules.Infrastructure.HttpClients.Mdm;
using Skeleton.CleanArchitecture.Application.Services.TokenBuilder;
using Skeleton.CleanArchitecture.DelegatingHandlers;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;
using Skeleton.CleanArchitecture.Infrastructure.HttpClients.Interfaces.Auth;
using Skeleton.CleanArchitecture.Infrastructure.Interfaces;
using System.Net.Http.Headers;

namespace Skeleton.CleanArchitecture.Extensions
{
    internal static class BusinessDependencyInjection
    {
        private const string ApplicationJsonMIMEType = "application/json";

        public static IServiceCollection AddBusinessInjection(
            this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.AddMdmHttpClient();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<ExternalEndpointClientCredentialDelegatingHandler>();
            services.TryDecorate<IExternalHttpClient, ExternalEndpointHttpClientCacheDecorator>();
            return services;
        }

        private static void AddMdmHttpClient(this IServiceCollection services)
        {
            const string MdmClient = nameof(MdmClient);

            services.AddHttpClient(MdmClient)
                .ConfigureHttpClient(CfgMdmClient)
                .AddHttpMessageHandler<ExternalEndpointClientCredentialDelegatingHandler>()
                .AddTypedClient<IExternalHttpClient, ExternalEndpointHttpClient>()
                .AddStanderResilienceHandler();

            static void CfgMdmClient(IServiceProvider provider, HttpClient client)
            {
                var config = provider.GetRequiredService<ExternalEndpointConfiguration>();
                client.BaseAddress = new Uri(config.ServiceBaseAddress);

                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJsonMIMEType));
            }
        }
    }
}
