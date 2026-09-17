using CRMSystem.Data;
using System.Diagnostics;

namespace CRMSystem.Models.Entities
{
    public class Opportunity
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid PipelineId { get; set; }

        public Guid StageId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public string Status { get; set; } = "OPEN";

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Organization Organization { get; set; } = null!;

        public Customer Customer { get; set; } = null!;

        public Pipeline Pipeline { get; set; } = null!;

        public PipelineStage Stage { get; set; } = null!;

        public ApplicationUser? AssignedUser { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
