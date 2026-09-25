namespace CRMSystem.Models.Entities
{
    public class LeadStatus
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = null!;

        public int SortOrder { get; set; }

        public bool IsClosed { get; set; }

        // Navigation
        public Organization Organization { get; set; } = null!;

        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
