using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IOpportunityServices
    {
        Task <IEnumerable<OpportunityResponseDto>> GetAllOpportunity();
        Task<OpportunityResponseDto> GetOpportunityById(Guid id);
        Task CreateOpportunity(CreateOpportunityDto opportunity);
        Task<OpportunityResponseDto> UpdateOpportunity(Guid id, UpdateOpportunityDto opportunity);
        Task DeleteOpportunity(Guid id);
        Task UpdateOpportunityStage(Guid id, Guid stageId);
        Task AssignOpportunity(Guid id, Guid? assignedUserId);
        Task UpdateOpportunityStatus(Guid id, string status);
    }
}
