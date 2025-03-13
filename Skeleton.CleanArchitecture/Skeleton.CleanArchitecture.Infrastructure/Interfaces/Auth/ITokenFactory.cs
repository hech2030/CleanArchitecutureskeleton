using Skeleton.CleanArchitecture.Domain.Entities.Common.Auth;

namespace Skeleton.CleanArchitecture.Infrastructure.HttpClients.Interfaces.Auth;

public interface ITokenFactory
{
    Task<TokenModel> RefereshToken(TokenModel token, string scope, CancellationToken cancellationToken);
}
