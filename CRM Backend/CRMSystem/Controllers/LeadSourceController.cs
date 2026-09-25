using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/lead-sources")]
    [Authorize]
    public class LeadSourcesController : BaseController
    {
        private readonly ILeadSourceServices _leadSourceServices;

        public LeadSourcesController(ILeadSourceServices leadSourceServices)
        {
            _leadSourceServices = leadSourceServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeadSource()
        {
            var leadSources = await _leadSourceServices.GetAllLeadSource();
            return Success("Lead sources retrieved successfully.", leadSources);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{leadSourceId:guid}")]
        public async Task<IActionResult> GetLeadSourceById(Guid leadSourceId)
        {
            var leadSource = await _leadSourceServices.GetLeadSourceById(leadSourceId);
            return Success("Lead source retrieved successfully.", leadSource);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateLeadSource(CreateLeadSourceDto leadsource)
        {
            await _leadSourceServices.CreateLeadSource(leadsource);
            return Created("Lead source created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{leadSourceId:guid}")]
        public async Task<IActionResult> DeleteLeadSource(Guid leadSourceId)
        {
            await _leadSourceServices.DeleteLeadSourceById(leadSourceId);
            return Success("Lead source deleted successfully.");
        }
    }
}