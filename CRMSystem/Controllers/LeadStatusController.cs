using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/lead-statuses")]
    [Authorize]
    public class LeadStatusController : ControllerBase
    {
        private readonly ILeadStatusServices _leadStatusServices;

        public LeadStatusController(ILeadStatusServices leadStatusServices)
        {
            _leadStatusServices = leadStatusServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadStatusResponseDto>>> GetAllLeadStatus()
        {
            var leadStatuses = await _leadStatusServices.GetAllLeadStatus();

            return Ok(leadStatuses);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LeadStatusResponseDto>> GetLeadStatusById(Guid id)
        {
            try
            {
                var leadStatus = await _leadStatusServices.GetLeadStatusById(id);

                return Ok(leadStatus);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Lead status not found."
                });
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateLeadStatus(CreateLeadStatusDto leadstatus)
        {
            await _leadStatusServices.CreateLeadStatus(leadstatus);

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Lead status created successfully."
            });
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLeadStatus(Guid id)
        {
            try
            {
                await _leadStatusServices.DeleteLeadStatusById(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Lead status not found."
                });
            }
        }
    }
}