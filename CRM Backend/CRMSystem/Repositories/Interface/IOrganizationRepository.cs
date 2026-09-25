using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllOrganization();
        Task<Organization> GetOrganizationById(Guid id);
        Task CreateOrganization(Organization organization);
        Task UpdateOrganization(Organization organization);
        Task DeleteOrganization(Guid id);
    }
}
