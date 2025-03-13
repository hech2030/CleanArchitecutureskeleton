using Azure.Core;
using Microsoft.Identity.Web;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Auth;
using Skeleton.CleanArchitecture.Infrastructure.HttpClients.Interfaces.Auth;

namespace Skeleton.CleanArchitecture.Application.Services.TokenBuilder
{
    public class TokenFactory : ITokenFactory
    {
        private const int DelayInMinutesBeforeTokenExpiration = -1;
        private readonly MicrosoftIdentityOptions azureAdConfig;

        public TokenFactory(MicrosoftIdentityOptions azureAdConfig)
        {
            this.azureAdConfig = azureAdConfig;
        }

        public async Task<TokenModel> RefereshToken(TokenModel token, string scope, CancellationToken cancellationToken)
        {
            if (token is null || AccessTokenExpired(token.CurrentTokenExpiresAt))
            {
                token = await AcquireAccessToken(scope, cancellationToken);
            }

            return token;
        }

        private static bool AccessTokenExpired(DateTimeOffset currentTokenExpiresAt) =>
            currentTokenExpiresAt <= DateTime.UtcNow.AddMinutes(DelayInMinutesBeforeTokenExpiration);

        private async Task<TokenModel> AcquireAccessToken(string scope, CancellationToken cancellationToken)
        {
            var certificate = azureAdConfig.GetAzureAdClientCertificate();

            ArgumentNullException.ThrowIfNull(certificate);

            var scopes = new string[] { scope };

            var token = await certificate.GetTokenAsync(new TokenRequestContext(scopes), cancellationToken);
            return new TokenModel(token.ExpiresOn, token.Token);
        }
    }
}
