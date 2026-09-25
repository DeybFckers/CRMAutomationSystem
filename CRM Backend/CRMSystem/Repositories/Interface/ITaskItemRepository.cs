using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTask(Guid organizationId);
        Task<TaskItem?> GetTaskById(Guid id, Guid organizationId);
        Task CreateTask(TaskItem task);
        Task UpdateTask(TaskItem task);
        Task DeleteTask(Guid id, Guid organizationId);
    }
}
