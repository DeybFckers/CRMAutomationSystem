namespace CRMSystem.Models.Entities
{
    public class PipelineStage
    {
        public Guid Id { get; set; }

        public Guid PipelineId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Probability { get; set; }

        public int SortOrder { get; set; }

        public bool IsWon { get; set; }

        public bool IsLost { get; set; }

        public Pipeline Pipeline { get; set; } = null!;

        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
}
