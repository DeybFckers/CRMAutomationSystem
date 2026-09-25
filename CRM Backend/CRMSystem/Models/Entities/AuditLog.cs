using CRMSystem.Data;

namespace CRMSystem.Models.Entities
{
    public class AuditLog
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

        public Organization Organization { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
