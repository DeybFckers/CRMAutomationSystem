using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/leads")]
    [Authorize]
    public class LeadsController : BaseController
    {
        private readonly ILeadServices _leadServices;

        public LeadsController(ILeadServices leadServices)
        {
            _leadServices = leadServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllLead(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] Guid? statusId = null,
            [FromQuery] Guid? sourceId = null,
            [FromQuery] Guid? assignedUserId = null,
            [FromQuery] Guid? customerId = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = null)
        {
            var leads = await _leadServices.GetAllLead(
                page,
                pageSize,
                search,
                statusId,
                sourceId,
                assignedUserId,
                customerId,
                sortBy,
                sortDirection);

            return Success("Leads retrieved successfully.", leads);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{leadId:guid}")]
        public async Task<IActionResult> GetLeadById(Guid leadId)
        {
            var lead = await _leadServices.GetLeadById(leadId);
            return Success("Lead retrieved successfully.", lead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateLead(CreateLeadDto lead)
        {
            var createdLead = await _leadServices.CreateLead(lead);
            return Created("Lead created successfully.", createdLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{leadId:guid}")]
        public async Task<IActionResult> UpdateLead(Guid leadId, UpdateLeadDto lead)
        {
            var updatedLead = await _leadServices.UpdateLead(leadId, lead);
            return Success("Lead updated successfully.", updatedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{leadId:guid}/assign")]
        public async Task<IActionResult> AssignLead(Guid leadId, AssignLeadDto lead)
        {
            var updatedLead = await _leadServices.AssignLead(leadId, lead);
            return Success("Lead assigned successfully.", updatedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{leadId:guid}/status")]
        public async Task<IActionResult> UpdateLeadStatus(Guid leadId, UpdateLeadStatusDto lead)
        {
            var updatedLead = await _leadServices.UpdateLeadStatus(leadId, lead);
            return Success("Lead status updated successfully.", updatedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost("{leadId:guid}/convert")]
        public async Task<IActionResult> ConvertLeadToCustomer(Guid leadId)
        {
            var convertedLead = await _leadServices.ConvertLeadToCustomer(leadId);
            return Success("Lead converted to customer successfully.", convertedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{leadId:guid}")]
        public async Task<IActionResult> DeleteLead(Guid leadId)
        {
            await _leadServices.DeleteLead(leadId);
            return Success("Lead deleted successfully.");
        }
    }
}