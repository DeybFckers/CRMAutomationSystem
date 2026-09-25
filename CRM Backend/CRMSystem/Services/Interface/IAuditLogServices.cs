using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IAuditLogServices
    {
        Task<AuditLogResponseDto?> GetAuditLogById(Guid auditLogId);
        Task<IEnumerable<AuditLogResponseDto>> GetAllAuditLogs();
    }
}