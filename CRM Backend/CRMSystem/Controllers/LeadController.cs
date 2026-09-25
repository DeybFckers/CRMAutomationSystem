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
        public async Task<IActionResult> GetAllLead()
        {
            var leads = await _leadServices.GetAllLead();
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
            await _leadServices.CreateLead(lead);
            return Created("Lead created successfully.");
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
            await _leadServices.ConvertLeadToCustomer(leadId);
            return Success("Lead converted to customer successfully.");
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