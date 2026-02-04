namespace Integrations.ModernService.Domain.Entities.Common.Auth;
public class TokenModel
{
    public TokenModel(DateTimeOffset currentTokenExpiresAt, string accessToken)
    {
        CurrentTokenExpiresAt = currentTokenExpiresAt;
        AccessToken = accessToken;
    }
    public DateTimeOffset CurrentTokenExpiresAt { get; }
    public string AccessToken { get; }
}
