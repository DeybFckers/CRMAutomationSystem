using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Route("api/opportunities")]
    [ApiController]
    [Authorize]
    public class OpportunityController : BaseController
    {
        private readonly IOpportunityServices _opportunityServices;

        public OpportunityController(IOpportunityServices opportunityServices)
        {
            _opportunityServices = opportunityServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateOpportunity(CreateOpportunityDto opportunity)
        {
            await _opportunityServices.CreateOpportunity(opportunity);
            return Created("Opportunity created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllOpportunity()
        {
            var opportunities = await _opportunityServices.GetAllOpportunity();
            return Success("Opportunities retrieved successfully.", opportunities);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{opportunityId:guid}")]
        public async Task<IActionResult> GetOpportunityById(Guid opportunityId)
        {
            var opportunity = await _opportunityServices.GetOpportunityById(opportunityId);
            return Success("Opportunity retrieved successfully.", opportunity);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{opportunityId:guid}")]
        public async Task<IActionResult> UpdateOpportunity(Guid opportunityId, UpdateOpportunityDto opportunity)
        {
            await _opportunityServices.UpdateOpportunity(opportunityId, opportunity);
            return Success("Opportunity updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{opportunityId:guid}")]
        public async Task<IActionResult> DeleteOpportunity(Guid opportunityId)
        {
            await _opportunityServices.DeleteOpportunity(opportunityId);
            return Success("Opportunity deleted successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{opportunityId:guid}/stage/{stageId:guid}")]
        public async Task<IActionResult> UpdateOpportunityStage(Guid opportunityId, Guid stageId)
        {
            await _opportunityServices.UpdateOpportunityStage(opportunityId, stageId);
            return Success("Opportunity stage updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{opportunityId:guid}/assign/{assignedUserId:guid}")]
        public async Task<IActionResult> AssignOpportunity(Guid opportunityId, Guid? assignedUserId)
        {
            await _opportunityServices.AssignOpportunity(opportunityId, assignedUserId);
            return Success("Opportunity assignment updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{opportunityId:guid}/status")]
        public async Task<IActionResult> UpdateOpportunityStatus(Guid opportunityId, string status)
        {
            await _opportunityServices.UpdateOpportunityStatus(opportunityId, status);
            return Success("Opportunity status updated successfully.");
        }
    }
}