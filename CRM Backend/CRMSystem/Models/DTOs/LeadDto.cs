namespace CRMSystem.Models.DTOs
{
    //LEAD
    public class LeadResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public UserMinimalDto? AssignedUser { get; set; }

        public LeadSourceMinimalDto Source { get; set; } = null!;
        public LeadStatusMinimalDto Status { get; set; } = null!;

        public Guid? CustomerId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public decimal? EstimatedValue { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LeadMinimalDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal? EstimatedValue { get; set; }
    }

    public class CreateLeadDto
    {
        public Guid? AssignedUserId { get; set; }

        public Guid SourceId { get; set; }
        public Guid StatusId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public decimal? EstimatedValue { get; set; }

        public string? Notes { get; set; }
    }

    public class UpdateLeadDto
    {
        public Guid? AssignedUserId { get; set; }

        public Guid SourceId { get; set; }
        public Guid StatusId { get; set; }

        public decimal? EstimatedValue { get; set; }

        public string? Notes { get; set; }
    }

    public class AssignLeadDto
    {
        public Guid AssignedUserId { get; set; }
    }

    public class UpdateLeadStatusDto
    {
        public Guid StatusId { get; set; }
    }
    
    //LEAD SOURCE
    public class LeadSourceResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class LeadSourceMinimalDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
    }

    public class CreateLeadSourceDto
    {
        public string Name { get; set; } = null!;
    }


    //LEAD STATUS
    public class LeadStatusResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = null!;

        public int SortOrder { get; set; }

        public bool IsClosed { get; set; }
    }

    public class LeadStatusMinimalDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int SortOrder { get; set; }

        public bool IsClosed { get; set; }
    }

    public class CreateLeadStatusDto
    {
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsClosed { get; set; }
    }
}
