using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog?> GetAuditLogById(Guid organizationId, Guid auditLogId)
        {
            return await _context.AuditLogs.FirstOrDefaultAsync(a => a.OrganizationId == organizationId && a.Id == auditLogId);
        }

        public async Task<IEnumerable<AuditLog>> GetAllAuditLogs(Guid organizationId)
        {
            return await _context.AuditLogs.Where(a => a.OrganizationId == organizationId).ToListAsync();
        }
    }
}