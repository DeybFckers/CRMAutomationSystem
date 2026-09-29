using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userServices.GetAllUsers();

            return Success("Users retrieved successfully.", users);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userServices.GetUserById(userId);

            return Success("User retrieved successfully.", user);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUserByRole(string role)
        {
            var users = await _userServices.GetUserByRole(role);

            return Success("Users retrieved successfully.", users);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _userServices.DeleteUser(userId);

            return Success("User deleted successfully.");
        }
    }
}