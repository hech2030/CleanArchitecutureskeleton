namespace Integrations.ModernService.Infrastructure.HttpClients.TokenBuilders.LinkedIn
{
    public class LinkedInTokenModel
    {
        /// <summary>
        /// The access token provided by LinkedIn.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// How long, in seconds, until the access token expires.
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
