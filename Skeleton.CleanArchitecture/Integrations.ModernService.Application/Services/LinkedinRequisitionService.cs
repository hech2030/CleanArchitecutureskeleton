using Integrations.ModernService.Domain.Entities;
using Integrations.ModernService.Infrastructure.Interfaces;

namespace Integrations.ModernService.Application.Services
{
    public class LinkedinRequisitionService : ILinkedinRequisitionService
    {
        public Task<LinkedInJobPostingStatusType> GetAndStoreJobStatusAsync(int requisitionId, CancellationToken cancellationToken)
        {
            return Task.FromResult(LinkedInJobPostingStatusType.Listed);
        }
    }
}
