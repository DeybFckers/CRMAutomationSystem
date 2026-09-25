using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateTask(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();    
        }

        public async Task DeleteTask(Guid id, Guid organizationId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (task == null)
            {
                throw new Exception("Task not found");
            }
            
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            
        }

        public async Task<IEnumerable<TaskItem>> GetAllTask(Guid organizationId)
        {
            return await _context.Tasks
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskById(Guid id, Guid organizationId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }
        

        public async Task UpdateTask(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
