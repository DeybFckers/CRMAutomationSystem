using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class LeadConversionRepository : ILeadConversionRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadConversionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<LeadConversion> LeadConversions, int TotalCount)> GetAllLeadConversions(Guid organizationId, int page, int pageSize)
        {
            var query = _context.LeadConversions
                .Where(x => x.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();

            var leadConversions = await query
                .Include(x => x.Lead)
                .Include(x => x.Customer)
                .Include(x => x.ConvertedByUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (leadConversions, totalCount);
        }

        public async Task<LeadConversion?> GetLeadConversionById(Guid id, Guid organizationId)
        {
            return await _context.LeadConversions
                .Include(x => x.Lead)
                .Include(x => x.Customer)
                .Include(x => x.ConvertedByUser)
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }

        public async Task UpdateLeadConversion(LeadConversion leadConversion)
        {
            _context.LeadConversions.Update(leadConversion);
            await _context.SaveChangesAsync();
        }
    }
}