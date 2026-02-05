using Integrations.ModernService.Domain.Entities;

namespace Integrations.ModernService.Infrastructure.Interfaces
{
    public interface ILinkedinRequisitionService
    {
        /// <summary>
        /// Get job status for a requisition.
        /// </summary>
        /// <param name="requisitionId">requisitionId</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the job posting status as a LinkedInJobPostingStatusType.</returns>
        Task<LinkedInJobPostingStatusType> GetAndStoreJobStatusAsync(int requisitionId, CancellationToken cancellationToken);
    }
}
