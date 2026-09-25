using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateActivity(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteActivity(Guid organizationId, Guid activityId)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.OrganizationId == organizationId && a.Id == activityId);

            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Activity?> GetActivityById(Guid organizationId, Guid activityId)
        {
            return await _context.Activities.FirstOrDefaultAsync(a => a.OrganizationId == organizationId && a.Id == activityId);
        }

        public async Task<IEnumerable<Activity>> GetAllActivity(Guid organizationId)
        {
            return await _context.Activities.Where(a => a.OrganizationId == organizationId).ToListAsync();
        }

        public async Task UpdateActivity(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }
    }
}