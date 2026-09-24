using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ActivityController : BaseController
    {
        private readonly IActivityServices _activityServices;

        public ActivityController(IActivityServices activityServices)
        {
            _activityServices = activityServices;
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateActivity(CreateActivityDto dto)
        {
            var activity = await _activityServices.CreateActivity(dto);

            return Created("Activity created successfully.", activity);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _activityServices.GetAllActivities();

            return Success("Activities retrieved successfully.", activities);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{activityId:guid}")]
        public async Task<IActionResult> GetActivityById(Guid activityId)
        {
            var activity = await _activityServices.GetActivityById(activityId);

            if (activity == null)
                throw new KeyNotFoundException("Activity not found.");

            return Success("Activity retrieved successfully.", activity);
        }
        
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{activityId:guid}")]
        public async Task<IActionResult> UpdateActivity(Guid activityId, UpdateActivityDto dto)
        {
            var activity = await _activityServices.UpdateActivity(activityId, dto);

            return Success("Activity updated successfully.", activity);
        }
        
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{activityId:guid}")]
        public async Task<IActionResult> DeleteActivity(Guid activityId)
        {
            await _activityServices.DeleteActivity(activityId);

            return Success("Activity deleted successfully.");
        }
    }
}