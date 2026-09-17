using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ILeadServices
    {
        Task<IEnumerable<LeadResponseDto>> GetAllLead();
        Task<LeadResponseDto> GetLeadById(Guid id);
        Task CreateLead(CreateLeadDto lead);
        Task ConvertLeadToCustomer(Guid id);
        Task<LeadResponseDto> UpdateLead(Guid id, UpdateLeadDto lead);
        Task<LeadResponseDto> AssignLead(Guid id, AssignLeadDto lead);
        Task<LeadResponseDto> UpdateLeadStatus(Guid id, UpdateLeadStatusDto lead);
        Task DeleteLead(Guid id);

    }
}
