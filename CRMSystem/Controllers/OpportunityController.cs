using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Route("api/opportunities")]
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly IOpportunityServices _opportunityServices;

        public OpportunityController(IOpportunityServices opportunityServices)
        {
            _opportunityServices = opportunityServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOpportunity(CreateOpportunityDto opportunity)
        {
            await _opportunityServices.CreateOpportunity(opportunity);
            return Ok(new { message = "Opportunity created successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOpportunity()
        {
            var opportunities = await _opportunityServices.GetAllOpportunity();
            return Ok(opportunities);
        }

        [HttpGet("{opportunityId:guid}")]
        public async Task<IActionResult> GetOpportunityById(Guid opportunityId)
        {
            var opportunity = await _opportunityServices.GetOpportunityById(opportunityId);
            return Ok(opportunity);
        }

        [HttpPut("{opportunityId:guid}")]
        public async Task<IActionResult> UpdateOpportunity(Guid opportunityId, UpdateOpportunityDto opportunity)
        {
            await _opportunityServices.UpdateOpportunity(opportunityId, opportunity);
            return Ok(new { message = "Opportunity updated successfully." });
        }

        [HttpDelete("{opportunityId:guid}")]
        public async Task<IActionResult> DeleteOpportunity(Guid opportunityId)
        {
            await _opportunityServices.DeleteOpportunity(opportunityId);
            return Ok(new { message = "Opportunity deleted successfully." });
        }

        [HttpPatch("{opportunityId:guid}/stage/{stageId:guid}")]
        public async Task<IActionResult> UpdateOpportunityStage(Guid opportunityId, Guid stageId)
        {
            await _opportunityServices.UpdateOpportunityStage(opportunityId, stageId);
            return Ok(new { message = "Opportunity stage updated successfully." });
        }

        [HttpPatch("{opportunityId:guid}/assign/{assignedUserId:guid}")]
        public async Task<IActionResult> AssignOpportunity(Guid opportunityId, Guid? assignedUserId)
        {
            await _opportunityServices.AssignOpportunity(opportunityId, assignedUserId);
            return Ok(new { message = "Opportunity assignment updated successfully." });
        }

        [HttpPatch("{opportunityId:guid}/status")]
        public async Task<IActionResult> UpdateOpportunityStatus(Guid opportunityId, string status)
        {
            await _opportunityServices.UpdateOpportunityStatus(opportunityId, status);
            return Ok(new { message = "Opportunity status updated successfully." });
        }
    }
}