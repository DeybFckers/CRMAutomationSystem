namespace CRMSystem.Models.DTOs
{
    public class ActivityResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public UserMinimalDto User { get; set; } = null!;
        public CustomerMinimalDto? Customer { get; set; }
        public LeadMinimalDto? Lead { get; set; }
        public OpportunityMinimalDto? Opportunity { get; set; }

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

    public class UpdateActivityDto
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
