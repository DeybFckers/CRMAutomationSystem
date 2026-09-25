using CRMSystem.Data;

namespace CRMSystem.Models.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

      
        public string TokenHash { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public Guid? ReplacedByTokenId { get; set; }

        public string? CreatedByIp { get; set; }

        public string? UserAgent { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public RefreshToken? ReplacedByToken { get; set; }
    }
}
