using CRMSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid? OrganizationId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Organization? Organization { get; set; } = null!;

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();

        public ICollection<Lead> Leads { get; set; } = new List<Lead>();

        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
