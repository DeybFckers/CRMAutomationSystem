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
    public class UserController : BaseController
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] Guid organizationId)
        {
            var users = await _userServices.GetAllUsers(organizationId);
            return Success("Users retrieved successfully.", users);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId, [FromQuery] Guid organizationId)
        {
            var user = await _userServices.GetUserById(userId, organizationId);

            if (user == null)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "User not found.", Data = null });
            }

            return Success("User retrieved successfully.", user);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUserByRole(string role, [FromQuery] Guid organizationId)
        {
            var users = await _userServices.GetUserByRole(role, organizationId);
            return Success("Users retrieved successfully.", users);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId, [FromQuery] Guid organizationId)
        {
            var deleted = await _userServices.DeleteUser(userId, organizationId);

            if (!deleted)
            {
                return NotFound(new ErrorResponse { Success = false, Message = "User not found.", Data = null });
            }

            return Success("User deleted successfully.");
        }
    }
}