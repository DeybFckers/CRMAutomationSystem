using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ILeadRepository
    {
        Task<(IEnumerable<Lead> Leads, int TotalCount)> GetAllLeads(Guid organizationId, int page, int pageSize, string? search, Guid? statusId, Guid? sourceId, Guid? assignedUserId, string? sortBy, string? sortDirection);

        Task<Lead?> GetLeadById(Guid id, Guid organizationId);

        Task CreateLead(Lead lead);

        Task UpdateLead(Lead lead);

        Task DeleteLead(Guid id, Guid organizationId);
        Task<LeadStatus?> GetStatusByName(Guid organizationId, string name);
        void UpdateLeadNoSave(Lead lead);

    }
}
