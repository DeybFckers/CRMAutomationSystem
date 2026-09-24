using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IAuditLogRepository
    {
        Task<AuditLog?> GetAuditLogById(Guid organizationId, Guid auditLogId);
        Task<IEnumerable<AuditLog>> GetAllAuditLogs(Guid organizationId);
    }
}