using CRMSystem.Models.DTOs;
using CRMSystem.Models.Responses;

namespace CRMSystem.Services.Interface
{
    public interface ILeadConversionServices
    {
        Task<PaginatedResponse<LeadConversionResponseDto>> GetAllLeadConversions(int page, int pageSize);
        Task<LeadConversionResponseDto> GetLeadConversionById(Guid id);
        Task<LeadConversionResponseDto> UpdateLeadConversion(Guid id, UpdateLeadConversionDto leadConversion);
    }
}