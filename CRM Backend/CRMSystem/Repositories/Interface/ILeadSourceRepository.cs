using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ILeadSourceRepository
    {
        Task<IEnumerable<LeadSource>> GetAllLeadSource(Guid organizationId);
        Task<LeadSource?> GetLeadSourceById(Guid id, Guid organizationId);
        Task CreateLeadSource(LeadSource leadSource);
        Task DeleteLeadSource(Guid id, Guid organizationId);

        Task<LeadSource?> GetSourceByName(string name, Guid organizationId); 
    }
}
