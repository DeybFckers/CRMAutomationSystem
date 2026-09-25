using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers(
            Organization organization)
        {
            return await _userManager.Users
                .Where(u => u.OrganizationId == organization.Id)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserById(Guid id, Organization organization)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u =>
                    u.Id == id &&
                    u.OrganizationId == organization.Id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRole( string role, Organization organization)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);

            return usersInRole
                .Where(u => u.OrganizationId == organization.Id)
                .ToList();
        }

        public async Task<bool> DeleteUser( Guid id, Guid organizationId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u =>
                    u.Id == id &&
                    u.OrganizationId == organizationId);

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}