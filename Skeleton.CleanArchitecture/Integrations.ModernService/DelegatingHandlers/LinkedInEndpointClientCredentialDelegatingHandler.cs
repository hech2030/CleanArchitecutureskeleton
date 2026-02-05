using Integrations.ModernService.Infrastructure.HttpClients.TokenBuilders.LinkedIn;
using System.Net.Http.Headers;

namespace Integrations.ModernService.DelegatingHandlers
{
    internal class LinkedInEndpointClientCredentialDelegatingHandler : DelegatingHandler
    {
        private readonly ILinkedInRetrieveTokenService _linkedInRetrieveTokenService;
        private readonly ILogger<LinkedInEndpointClientCredentialDelegatingHandler> _logger;
        private const string Bearer = nameof(Bearer);
        private readonly SemaphoreSlim tokenLock = new(1);
        

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public LinkedInEndpointClientCredentialDelegatingHandler(ILinkedInRetrieveTokenService LinkedInRetrieveTokenService,
                                                    ILogger<LinkedInEndpointClientCredentialDelegatingHandler> logger)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ArgumentNullException.ThrowIfNull(LinkedInRetrieveTokenService);
            ArgumentNullException.ThrowIfNull(logger);

            _linkedInRetrieveTokenService = LinkedInRetrieveTokenService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            LinkedInTokenModel myToken;
            await tokenLock.WaitAsync(cancellationToken);
            try
            {
                myToken = await _linkedInRetrieveTokenService.RetrieveToken(cancellationToken);
            }
            finally
            {
                tokenLock.Release();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(Bearer, myToken.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }


    }
}
