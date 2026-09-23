using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IActivityRepository
    {
        Task CreateActivity(Activity activity);
        Task DeleteActivity(Guid organizationId, Guid activityId);
        Task<Activity?> GetActivityById(Guid organizationId, Guid activityId);
        Task<IEnumerable<Activity>> GetAllActivity(Guid organizationId);
        Task UpdateActivity(Activity activity);
    }
}