using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuditLogController : BaseController
    {
        private readonly IAuditLogServices _auditLogServices;

        public AuditLogController(IAuditLogServices auditLogServices)
        {
            _auditLogServices = auditLogServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuditLogs()
        {
            var auditLogs = await _auditLogServices.GetAllAuditLogs();

            return Success("Audit logs retrieved successfully.", auditLogs);
        }

        [HttpGet("{auditLogId:guid}")]
        public async Task<IActionResult> GetAuditLogById(Guid auditLogId)
        {
            var auditLog = await _auditLogServices.GetAuditLogById(auditLogId);

            if (auditLog == null)
                throw new KeyNotFoundException("Audit log not found.");

            return Success("Audit log retrieved successfully.", auditLog);
        }
    }
}