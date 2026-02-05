namespace Integrations.ModernService.Infrastructure.HttpClients.TokenBuilders.LinkedIn
{
    public interface ILinkedInRetrieveTokenService
    {
        Task<LinkedInTokenModel> RetrieveToken(CancellationToken cancellationToken);
    }
}
