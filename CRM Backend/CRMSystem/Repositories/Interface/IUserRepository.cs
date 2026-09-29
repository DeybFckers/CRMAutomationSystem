using CRMSystem.Data;

namespace CRMSystem.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers(Guid organizationId);
        Task<ApplicationUser?> GetUserById(Guid userId, Guid organizationId);
        Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role, Guid organizationId);
        Task<bool> DeleteUser(Guid userId, Guid organizationId);
    }
}