using Integrations.ModernService.Infrastructure.Interfaces;
using Integrations.ModernService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Integrations.ModernService.Application.Services
{
    public class LinkedInIntegrationService : ILinkedInIntegrationService
    {
        private const string RecruiterSystemConnectEnabledSettingName = "linkedin.integrations.recruiter-system-connect.enabled";
        private const string ApplyConnectEnabledSettingName = "linkedin.integrations.apply-connect.enabled";
        private readonly ILinkedInEndpointHttpClient _externalHttpClient;
        private readonly AppDbContext _dbContext;
        public LinkedInIntegrationService(ILinkedInEndpointHttpClient externalHttpClient, AppDbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(externalHttpClient);
            ArgumentNullException.ThrowIfNull(dbContext);
            _externalHttpClient = externalHttpClient;
            _dbContext = dbContext;
        }

        public async Task<bool> IsIntegrationEnabled(int orgId, CancellationToken cancellationToken)
        {
            var rscSetting = _dbContext.OrganizationSettings.Where(s => s.OrganizationId == orgId &&
                            s.SettingName == RecruiterSystemConnectEnabledSettingName &&
                            s.SettingValue == "Y")
                .FirstOrDefaultAsync(cancellationToken);
            if(rscSetting is not null)
            {
                // call linkedin endpoint
                var status = await _externalHttpClient.GetIntegrationStatus("ORG_IDENTIFIER",
                    "CSA_API",
                    "RECRUITER",
                    cancellationToken);
                if(status == "ENABLED")
                {
                    return true;
                }
            }
            var applyConnectSetting = _dbContext.OrganizationSettings.Where(s => s.OrganizationId == orgId &&
                            s.SettingName == ApplyConnectEnabledSettingName &&
                            s.SettingValue == "Y");
            if (applyConnectSetting is not null)
            {
                // call linkedin endpoint
                var status = await _externalHttpClient.GetIntegrationStatus("<ORG_IDENTIFIER>",
                    "APPLY_CONNECT",
                    "JOBS",
                    cancellationToken);
                if (status == "ENABLED")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
