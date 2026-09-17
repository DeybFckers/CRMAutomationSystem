using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationServices _organizationServices;

        public OrganizationController(
            IOrganizationServices organizationServices)
        {
            _organizationServices = organizationServices;
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizations =
                await _organizationServices.GetAll();

            return Ok(organizations);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrganizationById(Guid id)
        {
            var organization =
                await _organizationServices.GetOrganizationById(id);

            if (organization == null)
            {
                return NotFound(new
                {
                    message = "Organization not found."
                });
            }

            return Ok(organization);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrganization(
            [FromBody] CreateOrganizationDto dto)
        {
            var organization =
                await _organizationServices.CreateOrganization(dto);

            return CreatedAtAction(
                nameof(GetOrganizationById),
                new { id = organization.Id },
                organization);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateOrganization(
            Guid id,
            [FromBody] UpdateOrganizationDto dto)
        {
            var updated =
                await _organizationServices
                    .UpdateOrganization(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Organization not found."
                });
            }

            return NoContent();
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrganization(Guid id)
        {
            var deleted =
                await _organizationServices.DeleteOrganization(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Organization not found."
                });
            }

            return NoContent();
        }
    }
}