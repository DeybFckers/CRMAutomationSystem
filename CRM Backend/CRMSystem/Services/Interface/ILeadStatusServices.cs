using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ILeadStatusServices
    {
        Task<IEnumerable<LeadStatusResponseDto>> GetAllLeadStatus();
        Task<LeadStatusResponseDto> GetLeadStatusById(Guid id);
        Task CreateLeadStatus(CreateLeadStatusDto leadstatus);
        Task DeleteLeadStatusById(Guid id);
    }
}