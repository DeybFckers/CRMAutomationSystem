using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/lead-conversions")]
    [Authorize]
    public class LeadConversionsController : BaseController
    {
        private readonly ILeadConversionServices _leadConversionServices;

        public LeadConversionsController(ILeadConversionServices leadConversionServices)
        {
            _leadConversionServices = leadConversionServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeadConversions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var leadConversions = await _leadConversionServices.GetAllLeadConversions(
                page,
                pageSize);

            return Success("Lead conversions retrieved successfully.", leadConversions);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{leadConversionId:guid}")]
        public async Task<IActionResult> GetLeadConversionById(Guid leadConversionId)
        {
            var leadConversion = await _leadConversionServices.GetLeadConversionById(leadConversionId);

            return Success("Lead conversion retrieved successfully.", leadConversion);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{leadConversionId:guid}")]
        public async Task<IActionResult> UpdateLeadConversion(Guid leadConversionId, UpdateLeadConversionDto leadConversion)
        {
            var updatedLeadConversion = await _leadConversionServices.UpdateLeadConversion(
                leadConversionId,
                leadConversion);

            return Success("Lead conversion updated successfully.", updatedLeadConversion);
        }
    }
}