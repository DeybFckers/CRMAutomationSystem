namespace CRMSystem.Models.DTOs
{
    public class OpportunityResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }
        public CustomerMinimalDto Customer { get; set; } = null!;
        public PipelineMinimalDto Pipeline { get; set; } = null!;
        public PipelineStageMinimalDto Stage { get; set; } = null!;
        public UserMinimalDto? AssignedUser { get; set; }

        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public string Status { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OpportunityMinimalDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Value { get; set; }
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
        public Guid CustomerId { get; set; }
        public Guid StageId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public string Status { get; set; } = "OPEN";

        public string? Description { get; set; }
    }

    public class UpdateOpportunityStageDto
    {
        public Guid StageId { get; set; }
    }

    public class AssignOpportunityDto
    {
        public Guid? AssignedUserId { get; set; }
    }

    public class UpdateOpportunityStatusDto
    {
        public string Status { get; set; } = null!;
    }
}
