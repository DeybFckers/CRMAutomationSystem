using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrganization(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrganization(Guid id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Organization>> GetAllOrganization()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization> GetOrganizationById(Guid id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task UpdateOrganization(Organization organization)
        {
            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();
        }
    }
}
