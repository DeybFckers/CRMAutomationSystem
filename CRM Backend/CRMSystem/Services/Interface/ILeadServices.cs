using CRMSystem.Models.DTOs;
using CRMSystem.Models.Responses;

namespace CRMSystem.Services.Interface
{
    public interface ILeadServices
    {
        Task<PaginatedResponse<LeadResponseDto>> GetAllLead(int page, int pageSize, string? search, Guid? statusId, Guid? sourceId, Guid? assignedUserId, Guid? customerId, string? sortBy, string? sortDirection);
        Task<LeadResponseDto> GetLeadById(Guid id);
        Task<LeadResponseDto> CreateLead(CreateLeadDto lead);
        Task<CustomerResponseDto> ConvertLeadToCustomer(Guid id);
        Task<LeadResponseDto> UpdateLead(Guid id, UpdateLeadDto lead);
        Task<LeadResponseDto> AssignLead(Guid id, AssignLeadDto lead);
        Task<LeadResponseDto> UpdateLeadStatus(Guid id, UpdateLeadStatusDto lead);
        Task DeleteLead(Guid id);

    }
}
