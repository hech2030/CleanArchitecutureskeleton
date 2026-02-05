using Integrations.ModernService.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Integrations.ModernService.Controllers;

[ApiController]
[Route("[controller]")]
public class LinkedInRequisitionController : ControllerBase
{
    private readonly ILogger<LinkedInRequisitionController> _logger;
    private readonly ILinkedinRequisitionService _linkedinRequisitionService;

    public LinkedInRequisitionController(
        ILogger<LinkedInRequisitionController> logger,
        ILinkedinRequisitionService linkedinRequisitionService
        )
    {
        _logger = logger;
        _linkedinRequisitionService = linkedinRequisitionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetIntegrationStatus(int requisitionId, CancellationToken cancellationToken = default)
    {
        var requisitionStatus = await _linkedinRequisitionService.GetAndStoreJobStatusAsync(requisitionId, cancellationToken);

        return Ok(requisitionStatus);
    }
}
