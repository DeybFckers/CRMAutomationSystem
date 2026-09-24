using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/lead-statuses")]
    [Authorize]
    public class LeadStatusController : BaseController
    {
        private readonly ILeadStatusServices _leadStatusServices;

        public LeadStatusController(ILeadStatusServices leadStatusServices)
        {
            _leadStatusServices = leadStatusServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeadStatus()
        {
            var leadStatuses = await _leadStatusServices.GetAllLeadStatus();
            return Success("Lead statuses retrieved successfully.", leadStatuses);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{leadStatusId:guid}")]
        public async Task<IActionResult> GetLeadStatusById(Guid leadStatusId)
        {
            var leadStatus = await _leadStatusServices.GetLeadStatusById(leadStatusId);
            return Success("Lead status retrieved successfully.", leadStatus);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateLeadStatus(CreateLeadStatusDto leadstatus)
        {
            await _leadStatusServices.CreateLeadStatus(leadstatus);
            return Created("Lead status created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{leadStatusId:guid}")]
        public async Task<IActionResult> DeleteLeadStatus(Guid leadStatusId)
        {
            await _leadStatusServices.DeleteLeadStatusById(leadStatusId);
            return Success("Lead status deleted successfully.");
        }
    }
}