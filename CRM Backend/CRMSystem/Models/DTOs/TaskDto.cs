namespace CRMSystem.Models.DTOs
{
    public class TaskItemResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid AssignedUserId { get; set; }

        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;

        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
    public class CreateTaskItemDto
    {
        public Guid AssignedUserId { get; set; }

        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Priority { get; set; } = "MEDIUM";
    }

    public class UpdateTaskItemDto
    {
        public Guid AssignedUserId { get; set; }

        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Priority { get; set; } = "MEDIUM";
    }

    public class UpdateTaskStatusDto
    {
        public string Status { get; set; } = null!;
    }

    public class AssignTaskDto
    {
        public Guid AssignedUserId { get; set; }
    }
}
