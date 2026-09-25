namespace CRMSystem.Models.Entities
{
    public class LeadSource
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public Organization Organization { get; set; } = null!;

        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
