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

        public async Task<(IEnumerable<Lead> Leads, int TotalCount)> GetAllLeads(Guid organizationId, int page, int pageSize, string? search, Guid? statusId, Guid? sourceId, Guid? assignedUserId, Guid? customerId, string? sortBy, string? sortDirection)
        {
            //query the all Leads but no filter
            var query = _context.Leads.Where(x => x.OrganizationId == organizationId);

            //if the status has a value then combine the query above and add a filter query
            if (statusId.HasValue)
            {   //filter query
                query = query.Where(x => x.StatusId == statusId.Value);
            }

            if (sourceId.HasValue)
                query = query.Where(x => x.SourceId == sourceId.Value);

            if (assignedUserId.HasValue)
                query = query.Where(x => x.AssignedUserId == assignedUserId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    (x.FirstName != null && x.FirstName.Contains(search)) ||
                    (x.LastName != null && x.LastName.Contains(search)) ||
                    (x.CompanyName != null && x.CompanyName.Contains(search)) ||
                    (x.Email != null && x.Email.Contains(search)) ||
                    (x.Phone != null && x.Phone.Contains(search)));
            }

            var totalCount = await query.CountAsync();

            sortBy = sortBy?.ToLower();
            sortDirection = sortDirection?.ToLower();

            switch (sortBy)
            {
                case "firstname":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.FirstName)
                        : query.OrderByDescending(x => x.FirstName);
                    break;

                case "lastname":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.LastName)
                        : query.OrderByDescending(x => x.LastName);
                    break;

                case "companyname":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.CompanyName)
                        : query.OrderByDescending(x => x.CompanyName);
                    break;

                case "email":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.Email)
                        : query.OrderByDescending(x => x.Email);
                    break;

                case "phone":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.Phone)
                        : query.OrderByDescending(x => x.Phone);
                    break;

                case "estimatedvalue":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.EstimatedValue)
                        : query.OrderByDescending(x => x.EstimatedValue);
                    break;

                case "updatedat":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.UpdatedAt)
                        : query.OrderByDescending(x => x.UpdatedAt);
                    break;

                case "createdat":
                default:
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.CreatedAt);
                    break;
            }

            var leads = await query
                .Include(x => x.AssignedUser)
                .Include(x => x.Source)
                .Include(x => x.Status)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (leads, totalCount);

        }


        public async Task<Lead?> GetLeadById(Guid id, Guid organizationId)
        {
            return await _context.Leads
                .Include(x => x.AssignedUser)
                .Include(x => x.Source)
                .Include(x => x.Status)
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