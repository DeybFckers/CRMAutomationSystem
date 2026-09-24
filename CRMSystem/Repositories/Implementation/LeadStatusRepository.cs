using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class LeadStatusRepository : ILeadStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeadStatus>> GetAllStatus(Guid organizationId)
        {
            return await _context.LeadStatuses
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<LeadStatus?> GetLeadStatusById(Guid id, Guid organizationId)
        {
            return await _context.LeadStatuses
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }

        public async Task CreateLeadStatus(LeadStatus leadstatus)
        {
            await _context.LeadStatuses.AddAsync(leadstatus);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeadStatus(Guid id, Guid organizationId)
        {
            var leadStatus = await _context.LeadStatuses
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (leadStatus == null)
                throw new KeyNotFoundException("Lead status not found.");

            _context.LeadStatuses.Remove(leadStatus);
            await _context.SaveChangesAsync();
        }

        public async Task<LeadStatus?> GetStatusByName(string name, Guid organizationId)
        {
            var status = _context.LeadStatuses.FirstOrDefault(x =>x.Name == name && x.OrganizationId == organizationId);

            if (status != null)
            {
                return status;
            }

            return null;
        }
    }
}