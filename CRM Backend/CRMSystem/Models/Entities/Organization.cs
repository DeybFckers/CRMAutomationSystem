using CRMSystem.Data;

namespace CRMSystem.Models.Entities
{
    public class Organization
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();

        public ICollection<Lead> Leads { get; set; } = new List<Lead>();

        public ICollection<LeadSource> LeadSources { get; set; } = new List<LeadSource>();

        public ICollection<LeadStatus> LeadStatuses { get; set; } = new List<LeadStatus>();

        public ICollection<Pipeline> Pipelines { get; set; } = new List<Pipeline>();

        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}