using CRMSystem.Models.DTOs;
using CRMSystem.Models.Responses;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationServices _organizationServices;

        public OrganizationController(IOrganizationServices organizationServices)
        {
            _organizationServices = organizationServices;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizations = await _organizationServices.GetAll();
            return Success("Organizations retrieved successfully.", organizations);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{organizationId:guid}")]
        public async Task<IActionResult> GetOrganizationById(Guid organizationId)
        {
            var organization = await _organizationServices.GetOrganizationById(organizationId);

            if (organization == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Organization not found.", Data = null });
            }

            return Success("Organization retrieved successfully.", organization);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationDto dto)
        {
            var organization = await _organizationServices.CreateOrganization(dto);
            return Created("Organization created successfully.", organization);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{organizationId:guid}")]
        public async Task<IActionResult> UpdateOrganization(Guid organizationId, UpdateOrganizationDto dto)
        {
            var updated = await _organizationServices.UpdateOrganization(organizationId, dto);

            if (!updated)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Organization not found.", Data = null });
            }

            return Success("Organization updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{organizationId:guid}")]
        public async Task<IActionResult> DeleteOrganization(Guid organizationId)
        {
            var deleted = await _organizationServices.DeleteOrganization(organizationId);

            if (!deleted)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "Organization not found.", Data = null });
            }

            return Success("Organization deleted successfully.");
        }
    }
}