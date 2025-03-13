using Microsoft.Extensions.Options;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Auth;
using Skeleton.CleanArchitecture.Domain.Entities.Common.Options;
using Skeleton.CleanArchitecture.Infrastructure.HttpClients.Interfaces.Auth;
using System.Net.Http.Headers;

namespace Skeleton.CleanArchitecture.DelegatingHandlers
{
    internal class ExternalEndpointClientCredentialDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenFactory _tokenFactroy;
        private readonly ILogger<ExternalEndpointClientCredentialDelegatingHandler> _logger;
        private TokenModel mdmToken;
        private const string Bearer = nameof(Bearer);
        private readonly ExternalEndpointConfiguration config;
        private readonly SemaphoreSlim tokenLock = new(1);
        

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ExternalEndpointClientCredentialDelegatingHandler(IOptions<ExternalEndpointConfiguration> config, 
                                                    ITokenFactory tokenFactroy,
                                                    ILogger<ExternalEndpointClientCredentialDelegatingHandler> logger)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ArgumentNullException.ThrowIfNull(config.Value);
            ArgumentNullException.ThrowIfNull(tokenFactroy);
            ArgumentNullException.ThrowIfNull(logger);

            this.config = config.Value;
            _tokenFactroy = tokenFactroy;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await tokenLock.WaitAsync(cancellationToken);
            try
            {
                mdmToken = await _tokenFactroy.RefereshToken(mdmToken, config.Scope, cancellationToken);
            }
            finally
            {
                tokenLock.Release();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(Bearer, mdmToken.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
