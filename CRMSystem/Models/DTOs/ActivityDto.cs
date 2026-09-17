namespace CRMSystem.Models.DTOs
{
    public class ActivityResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }

        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Type { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime ActivityDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateActivityDto
    {
        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Type { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime ActivityDate { get; set; }
    }
}
