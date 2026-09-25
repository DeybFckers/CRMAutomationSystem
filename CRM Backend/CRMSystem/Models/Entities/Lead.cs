using CRMSystem.Data;
using System.Diagnostics;

namespace CRMSystem.Models.Entities
{
    public class Lead
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid CreatedByUserId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public Guid SourceId { get; set; }

        public Guid StatusId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? CompanyName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public decimal? EstimatedValue { get; set; }

        public string? Notes { get; set; }

        public Guid? ConvertedCustomerId { get; set; }

        public DateTime? ConvertedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Organization Organization { get; set; } = null!;

        public ApplicationUser? AssignedUser { get; set; }

        public LeadSource Source { get; set; } = null!;

        public LeadStatus Status { get; set; } = null!;

        public Customer? ConvertedCustomer { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<Note> NoteEntries { get; set; } = new List<Note>();
    }
}
