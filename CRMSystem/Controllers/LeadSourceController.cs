using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/lead-sources")]
    [Authorize]
    public class LeadSourcesController : ControllerBase
    {
        private readonly ILeadSourceServices _leadSourceServices;

        public LeadSourcesController(ILeadSourceServices leadSourceServices)
        {
            _leadSourceServices = leadSourceServices;
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadSourceResponseDto>>> GetAllLeadSource()
        {
            var leadSources = await _leadSourceServices.GetAllLeadSource();

            return Ok(leadSources);
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LeadSourceResponseDto>> GetLeadSourceById(Guid id)
        {
            try
            {
                var leadSource = await _leadSourceServices.GetLeadSourceById(id);

                return Ok(leadSource);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Lead source not found."
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateLeadSource(CreateLeadSourceDto leadsource)
        {
            await _leadSourceServices.CreateLeadSource(leadsource);

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Lead source created successfully."
            });
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLeadSource(Guid id)
        {
            try
            {
                await _leadSourceServices.DeleteLeadSourceById(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Lead source not found."
                });
            }
        }
    }
}