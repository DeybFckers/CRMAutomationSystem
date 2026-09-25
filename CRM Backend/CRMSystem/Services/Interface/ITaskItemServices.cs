using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ITaskItemServices
    {
        Task<IEnumerable<TaskItemResponseDto>> GetAllTask(Guid organizationId);
        Task<TaskItemResponseDto?> GetTaskById(Guid id, Guid organizationId);
        Task CreateTask(CreateTaskItemDto task);
        Task UpdateTask(Guid id, UpdateTaskItemDto task);
        Task DeleteTask(Guid id, Guid organizationId);
        Task CompleteTask(Guid id, Guid organizationId);
        Task UpdateTaskStatus(Guid id, Guid organizationId, string status);
        Task AssignTask(Guid id, Guid organizationId, Guid assignedUserId);

    }
}
