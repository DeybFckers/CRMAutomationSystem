using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly ApplicationDbContext _context;

        public OpportunityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOpportunity(Opportunity opportunity)
        {
            await _context.Opportunities.AddAsync(opportunity);   
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOpportunity(Guid id)
        {
            var opportunity = await _context.Opportunities.FirstOrDefaultAsync(o => o.Id == id);
            if (opportunity == null)
            {
                throw new KeyNotFoundException("Opportunity not found.");
            }

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Opportunity>> GetAllOpportunity(Guid organizationId)
        {
            return await _context.Opportunities.Where(o => o.OrganizationId == organizationId).ToListAsync();
        }

        public async Task<Opportunity?> GetOpportunityById(Guid id, Guid organizationId)
        {
            return await _context.Opportunities.FirstOrDefaultAsync(o => o.Id == id && o.OrganizationId == organizationId);
        }

        public async Task UpdateOpportunity(Opportunity opportunity)
        {
            opportunity.UpdatedAt = DateTime.UtcNow;
            _context.Opportunities.Update(opportunity);
            await _context.SaveChangesAsync();
        }
    }
}
