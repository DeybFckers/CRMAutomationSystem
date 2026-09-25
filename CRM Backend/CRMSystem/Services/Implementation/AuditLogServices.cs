using CRMSystem.Models.DTOs;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class AuditLogServices : IAuditLogServices
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public AuditLogServices(IAuditLogRepository auditLogRepository, ICurrentUserServices currentUserServices)
        {
            _auditLogRepository = auditLogRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<AuditLogResponseDto?> GetAuditLogById(Guid auditLogId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var auditLog = await _auditLogRepository.GetAuditLogById(organizationId, auditLogId);

            return auditLog?.Adapt<AuditLogResponseDto>();
        }

        public async Task<IEnumerable<AuditLogResponseDto>> GetAllAuditLogs()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var auditLogs = await _auditLogRepository.GetAllAuditLogs(organizationId);

            return auditLogs.Adapt<IEnumerable<AuditLogResponseDto>>();
        }
    }
}