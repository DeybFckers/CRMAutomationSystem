using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lead>> GetAllLeads(Guid organizationId)
        {
            return await _context.Leads
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<Lead?> GetLeadById(Guid id, Guid organizationId)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }

        public async Task CreateLead(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLead(Lead lead)
        {
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLead(Guid id, Guid organizationId)
        {
            var lead = await _context.Leads
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (lead == null)
                throw new KeyNotFoundException("Lead not found.");

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
        }

        public async Task<LeadStatus?> GetStatusByName(Guid organizationId, string name)
        {
            return await _context.LeadStatuses
                .FirstOrDefaultAsync(x => x.OrganizationId == organizationId && x.Name == name);
        }

        public void UpdateLeadNoSave(Lead lead)
        {
            _context.Leads.Update(lead); // stages the change, caller controls SaveChangesAsync
        }
    }
}