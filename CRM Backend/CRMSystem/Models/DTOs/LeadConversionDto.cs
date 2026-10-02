namespace CRMSystem.Models.DTOs
{
    public class LeadConversionResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public LeadMinimalDto Lead { get; set; }
        public CustomerMinimalDto Customer { get; set; }
        public UserMinimalDto ConvertedByUser { get; set; }
        public DateTime ConvertedAt { get; set; }
    }

    public class UpdateLeadConversionDto
    {
        public Guid ConvertedByUserId { get; set; }

        public DateTime ConvertedAt { get; set; }
    }
}
