using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ILeadSourceServices
    {
        Task<IEnumerable<LeadSourceResponseDto>> GetAllLeadSource();
        Task<LeadSourceResponseDto> GetLeadSourceById(Guid id);
        Task CreateLeadSource(CreateLeadSourceDto leadsource);
        Task DeleteLeadSourceById(Guid id);
    }
}
