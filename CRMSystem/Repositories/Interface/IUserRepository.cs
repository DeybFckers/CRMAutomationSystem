using CRMSystem.Data;
using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers(Organization organization);
        Task<ApplicationUser?> GetUserById(Guid id, Organization organization);
        Task<IEnumerable<ApplicationUser>> GetUsersByRole( string role,Organization organization);
        Task<bool> DeleteUser( Guid id,Guid organizationId);
    }
}