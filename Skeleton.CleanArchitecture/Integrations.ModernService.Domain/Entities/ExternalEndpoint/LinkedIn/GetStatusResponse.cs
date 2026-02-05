namespace Integrations.ModernService.Domain.Entities.ExternalEndpoint.LinkedIn
{
    public class GetStatusResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// The result for with requested information.
        /// </summary>
        public Dictionary<string, TEntity> Results { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GetStatusResponse()
        {
            Results = new Dictionary<string, TEntity>();
        }
    }
}
