namespace CRMSystem.Models.Entities
{
    public class Pipeline
    {
        public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }

    public Organization Organization { get; set; } = null!;

    public ICollection<PipelineStage> Stages { get; set; } = new List<PipelineStage>();

    public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
}
