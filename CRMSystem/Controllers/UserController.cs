using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers( [FromQuery] Guid organizationId)
        {
            var users = await _userServices.GetAllUsers(organizationId);

            return Ok(users);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById( Guid id,[FromQuery] Guid organizationId)
        {
            var user = await _userServices.GetUserById( id,organizationId);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUserByRole(string role,[FromQuery] Guid organizationId)
        {
            var users = await _userServices.GetUserByRole(role,organizationId);

            return Ok(users);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id, [FromQuery] Guid organizationId)
        {
            var deleted = await _userServices.DeleteUser(id,organizationId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "User deleted successfully."
            });
        }
    }
}