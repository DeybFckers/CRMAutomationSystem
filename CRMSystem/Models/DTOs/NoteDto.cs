namespace CRMSystem.Models.DTOs
{
    public class NoteResponseDto
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
    }

    public class CreateNoteDto
    {
        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Content { get; set; } = null!;
    }

    public class UpdateNoteDto
    {
        public Guid? CustomerId { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? OpportunityId { get; set; }

        public string Content { get; set; } = null!;
    }
}
