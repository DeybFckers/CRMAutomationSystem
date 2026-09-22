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
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLeadById(Guid id)
        {
            var lead = await _leadServices.GetLeadById(id);
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
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateLead(Guid id, UpdateLeadDto lead)
        {
            var updatedLead = await _leadServices.UpdateLead(id, lead);
            return Success("Lead updated successfully.", updatedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{id:guid}/assign")]
        public async Task<IActionResult> AssignLead(Guid id, AssignLeadDto lead)
        {
            var updatedLead = await _leadServices.AssignLead(id, lead);
            return Success("Lead assigned successfully.", updatedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateLeadStatus(Guid id, UpdateLeadStatusDto lead)
        {
            var updatedLead = await _leadServices.UpdateLeadStatus(id, lead);
            return Success("Lead status updated successfully.", updatedLead);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost("{id:guid}/convert")]
        public async Task<IActionResult> ConvertLeadToCustomer(Guid id)
        {
            await _leadServices.ConvertLeadToCustomer(id);
            return Success("Lead converted to customer successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            await _leadServices.DeleteLead(id);
            return Success("Lead deleted successfully.");
        }
    }
}