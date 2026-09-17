using CRMSystem.Data;

namespace CRMSystem.Models.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid AssignedUserId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? LeadId { get; set; }

        public Guid? OpportunityId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Priority { get; set; } = "MEDIUM";

        public string Status { get; set; } = "PENDING";

        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public Organization Organization { get; set; } = null!;

        public ApplicationUser AssignedUser { get; set; } = null!;

        public Customer? Customer { get; set; }

        public Lead? Lead { get; set; }

        public Opportunity? Opportunity { get; set; }
    }
}
