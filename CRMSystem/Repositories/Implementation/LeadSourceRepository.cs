using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class LeadSourceRepository : ILeadSourceRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadSourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeadSource>> GetAllLeadSource(Guid organizationId)
        {
            return await _context.LeadSources
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<LeadSource?> GetLeadSourceById(Guid id, Guid organizationId)
        {
            return await _context.LeadSources
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }

        public async Task CreateLeadSource(LeadSource leadSource)
        {
            await _context.LeadSources.AddAsync(leadSource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeadSource(Guid id, Guid organizationId)
        {
            var leadSource = await _context.LeadSources
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (leadSource == null)
                throw new KeyNotFoundException("Lead source not found.");

            _context.LeadSources.Remove(leadSource);
            await _context.SaveChangesAsync();
        }
    }
}