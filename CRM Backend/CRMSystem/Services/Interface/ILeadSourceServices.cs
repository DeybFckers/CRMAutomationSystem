using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ILeadSourceServices
    {
        Task<IEnumerable<LeadSourceResponseDto>> GetAllLeadSource();
        Task<LeadSourceResponseDto> GetLeadSourceById(Guid id);
        Task<LeadSourceResponseDto> CreateLeadSource(CreateLeadSourceDto leadsource);
        Task DeleteLeadSourceById(Guid id);
    }
}
