using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

namespace Skeleton.CleanArchitecture.Application.Services.TokenBuilder
{
    internal static class ConfigurationExtensions
    {
        internal static TokenCredential GetAzureAdClientCertificate(this IConfiguration configuration)
            => configuration.GetSection("AzureAd").Get<MicrosoftIdentityOptions>()!.GetAzureAdClientCertificate();

        internal static TokenCredential GetAzureAdClientCertificate(this MicrosoftIdentityOptions azureAdConfig)
        {
            ArgumentNullException.ThrowIfNull(azureAdConfig);

#pragma warning disable CS8604 // Possible null reference argument.
            DefaultCertificateLoader.LoadFirstCertificate(azureAdConfig.ClientCertificates);
#pragma warning restore CS8604 // Possible null reference argument.
            CertificateDescription certificateDescription = azureAdConfig.ClientCertificates?.FirstOrDefault()
                ?? throw new CredentialUnavailableException($"AAD Client Certificate wasn't found, while searching for the first available client certificate by configuration.");

            return new ClientCertificateCredential(
                azureAdConfig.TenantId,
                azureAdConfig.ClientId,
                certificateDescription.Certificate);
        }
    }
}
