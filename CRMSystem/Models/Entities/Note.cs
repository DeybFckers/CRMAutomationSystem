using CRMSystem.Data;

namespace CRMSystem.Models.Entities
{
    public class Note
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid UserId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? LeadId { get; set; }

        public Guid? OpportunityId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Organization Organization { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public Customer? Customer { get; set; }

        public Lead? Lead { get; set; }

        public Opportunity? Opportunity { get; set; }
    }
}
