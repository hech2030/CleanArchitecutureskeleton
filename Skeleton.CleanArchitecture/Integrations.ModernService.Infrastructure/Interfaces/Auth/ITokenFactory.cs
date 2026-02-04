using Integrations.ModernService.Domain.Entities.Common.Auth;

namespace Integrations.ModernService.Infrastructure.HttpClients.Interfaces.Auth;

public interface ITokenFactory
{
    Task<TokenModel> RefereshToken(TokenModel token, string scope, CancellationToken cancellationToken);
}
