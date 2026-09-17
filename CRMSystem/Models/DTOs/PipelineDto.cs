namespace CRMSystem.Models.DTOs
{
    public class PipelineResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<PipelineStageResponseDto> Stages { get; set; } = [];
    }
    public class CreatePipelineDto
    {
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class PipelineStageResponseDto
    {
        public Guid Id { get; set; }
        public Guid PipelineId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Probability { get; set; }

        public int SortOrder { get; set; }

        public bool IsWon { get; set; }
        public bool IsLost { get; set; }
    }

    public class CreatePipelineStageDto
    {
        public string Name { get; set; } = null!;
        public decimal Probability { get; set; }
        public int SortOrder { get; set; }
        public bool IsWon { get; set; }
        public bool IsLost { get; set; }
    }

}
