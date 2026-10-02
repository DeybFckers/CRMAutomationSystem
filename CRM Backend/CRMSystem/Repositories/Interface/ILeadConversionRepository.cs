using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ILeadConversionRepository
    {
        Task<(IEnumerable<LeadConversion> LeadConversions, int TotalCount)> GetAllLeadConversions(Guid organizationId, int page, int pageSize);
        Task<LeadConversion?> GetLeadConversionById(Guid id, Guid organizationId);
        Task UpdateLeadConversion(LeadConversion leadConversion);
    }
}
