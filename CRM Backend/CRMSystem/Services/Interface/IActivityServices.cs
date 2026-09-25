using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IActivityServices
    {
        Task<ActivityResponseDto> CreateActivity(CreateActivityDto dto);
        Task DeleteActivity(Guid activityId);
        Task<ActivityResponseDto?> GetActivityById(Guid activityId);
        Task<IEnumerable<ActivityResponseDto>> GetAllActivities();
        Task<ActivityResponseDto> UpdateActivity(Guid activityId, UpdateActivityDto dto);
    }
}