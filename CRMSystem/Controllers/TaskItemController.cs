using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : BaseController
    {
        private readonly ITaskItemServices _taskItemServices;
        private readonly ICurrentUserServices _currentUserServices;

        public TasksController(ITaskItemServices taskItemServices, ICurrentUserServices currentUserServices)
        {
            _taskItemServices = taskItemServices;
            _currentUserServices = currentUserServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskItemDto task)
        {
            await _taskItemServices.CreateTask(task);
            return Created("Task created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllTask()
        {
            var tasks = await _taskItemServices.GetAllTask(_currentUserServices.OrganizationId);
            return Success("Tasks retrieved successfully.", tasks);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{taskItemId:guid}")]
        public async Task<IActionResult> GetTaskById(Guid taskItemId)
        {
            var task = await _taskItemServices.GetTaskById(taskItemId, _currentUserServices.OrganizationId);
            return Success("Task retrieved successfully.", task);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{taskItemId:guid}")]
        public async Task<IActionResult> UpdateTask(Guid taskItemId, UpdateTaskItemDto task)
        {
            await _taskItemServices.UpdateTask(taskItemId, task);
            return Success("Task updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{taskItemId:guid}")]
        public async Task<IActionResult> DeleteTask(Guid taskItemId)
        {
            await _taskItemServices.DeleteTask(taskItemId, _currentUserServices.OrganizationId);
            return Success("Task deleted successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{taskItemId:guid}/complete")]
        public async Task<IActionResult> CompleteTask(Guid taskItemId)
        {
            await _taskItemServices.CompleteTask(taskItemId, _currentUserServices.OrganizationId);
            return Success("Task completed successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{taskItemId:guid}/status")]
        public async Task<IActionResult> UpdateTaskStatus(Guid taskItemId, UpdateTaskStatusDto dto)
        {
            await _taskItemServices.UpdateTaskStatus(taskItemId, _currentUserServices.OrganizationId, dto.Status);
            return Success("Task status updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPatch("{taskItemId:guid}/assign")]
        public async Task<IActionResult> AssignTask(Guid taskItemId, AssignTaskDto dto)
        {
            await _taskItemServices.AssignTask(taskItemId, _currentUserServices.OrganizationId, dto.AssignedUserId);
            return Success("Task assigned successfully.");
        }
    }
}