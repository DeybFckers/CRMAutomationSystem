using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ITaskItemServices
    {
        Task<IEnumerable<TaskItemResponseDto>> GetAllTask();
        Task<TaskItemResponseDto?> GetTaskById(Guid id);
        Task<TaskItemResponseDto> CreateTask(CreateTaskItemDto task);
        Task UpdateTask(Guid id, UpdateTaskItemDto task);
        Task DeleteTask(Guid id);
        Task CompleteTask(Guid id);
        Task UpdateTaskStatus(Guid id, string status);
        Task AssignTask(Guid id, Guid assignedUserId);

    }
}
