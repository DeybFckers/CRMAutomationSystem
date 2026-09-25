using CRMSystem.Data;
using System.Diagnostics;

namespace CRMSystem.Models.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string CustomerCode { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? CompanyName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Organization Organization { get; set; } = null!;

        public ApplicationUser? AssignedUser { get; set; }

        public ICollection<CustomerContact> Contacts { get; set; } = new List<CustomerContact>();

        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();

        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
