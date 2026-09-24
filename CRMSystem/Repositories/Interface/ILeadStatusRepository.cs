using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ILeadStatusRepository
    {
        Task<IEnumerable<LeadStatus>> GetAllStatus(Guid organizationId);
        Task<LeadStatus?> GetLeadStatusById(Guid id, Guid organizationId);
        Task CreateLeadStatus(LeadStatus leadstatus);
        Task DeleteLeadStatus(Guid id, Guid organizationId);
        Task<LeadStatus?> GetStatusByName(string name, Guid organizationId);
    }
}