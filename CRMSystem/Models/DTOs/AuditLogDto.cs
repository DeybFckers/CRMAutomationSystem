namespace CRMSystem.Models.DTOs
{
    public class AuditLogResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }

        public string EntityType { get; set; } = null!;
        public Guid EntityId { get; set; }

        public string Action { get; set; } = null!;

        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
