namespace CRMSystem.Models.DTOs
{
    public class OpportunityResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid CustomerId { get; set; }

        public Guid PipelineId { get; set; }
        public Guid StageId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public string Status { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateOpportunityDto
    {
        public Guid CustomerId { get; set; }

        public Guid PipelineId { get; set; }
        public Guid StageId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public string? Description { get; set; }
    }

    public class UpdateOpportunityDto
    {
        public Guid StageId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public string Status { get; set; } = "OPEN";

        public string? Description { get; set; }
    }
}
