using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/leads")]
    [Authorize]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadServices _leadServices;

        public LeadsController(ILeadServices leadServices)
        {
            _leadServices = leadServices;
        }
        [Authorize(Roles ="SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadResponseDto>>> GetAllLead()
        {
            var leads = await _leadServices.GetAllLead();

            return Ok(leads);
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LeadResponseDto>> GetLeadById(Guid id)
        {
            try
            {
                var lead = await _leadServices.GetLeadById(id);

                return Ok(lead);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lead not found." });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateLead(CreateLeadDto lead)
        {
            try
            {
                await _leadServices.CreateLead(lead);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    message = "Lead created successfully."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<LeadResponseDto>> UpdateLead(Guid id, UpdateLeadDto lead)
        {
            try
            {
                var updatedLead = await _leadServices.UpdateLead(id, lead);

                return Ok(updatedLead);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lead not found." });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{id:guid}/assign")]
        public async Task<ActionResult<LeadResponseDto>> AssignLead(Guid id, AssignLeadDto lead)
        {
            try
            {
                var updatedLead = await _leadServices.AssignLead(id, lead);

                return Ok(updatedLead);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<LeadResponseDto>> UpdateLeadStatus(Guid id, UpdateLeadStatusDto lead)
        {
            try
            {
                var updatedLead = await _leadServices.UpdateLeadStatus(id, lead);

                return Ok(updatedLead);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost("{id:guid}/convert")]
        public async Task<IActionResult>
            ConvertLeadToCustomer(Guid id)
        {
            try
            {
                await _leadServices.ConvertLeadToCustomer(id);

                return Ok(new
                {
                    message ="Lead converted to customer successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }



        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            try
            {
                await _leadServices.DeleteLead(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lead not found." });
            }
        }
    }
}