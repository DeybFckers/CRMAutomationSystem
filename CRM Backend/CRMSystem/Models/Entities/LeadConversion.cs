using CRMSystem.Data;

namespace CRMSystem.Models.Entities
{
    public class LeadConversion
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid LeadId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid ConvertedByUserId { get; set; }

        public DateTime ConvertedAt { get; set; }

        // Navigation properties
        public Organization Organization { get; set; } = null!;

        public Lead Lead { get; set; } = null!;

        public Customer Customer { get; set; } = null!;

        public ApplicationUser? ConvertedByUser { get; set; }
    }
}