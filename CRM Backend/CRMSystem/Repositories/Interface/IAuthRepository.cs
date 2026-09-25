using CRMSystem.Data;
using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmail(string email);
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);
        Task<bool> CheckPassword(ApplicationUser user, string password);
        Task<IdentityResult> AddRolesToUser(ApplicationUser user, IEnumerable<string> roles);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task SaveRefreshToken(RefreshToken token);
        Task<RefreshToken?> GetRefreshToken(string hashedToken);
        Task RevokeRefreshToken(RefreshToken token);
    }
}
