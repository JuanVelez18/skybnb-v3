using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(TokenValue), IsUnique = true)]
    [Index(nameof(UserId))]
    public class RefreshTokens
    {
        public long Id { get; set; }

        [Required]
        public string TokenValue { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        [Required]
        public bool Used { get; set; } = false;

        public long? ReplacedByTokenId { get; set; }


        public Users? User { get; set; }
        public RefreshTokens? ReplacedByToken { get; set; }
        public List<RefreshTokens> ReplacesTokens { get; set; } = [];


        [NotMapped]
        public bool IsActive => RevokedAt == null && !Used && DateTime.UtcNow < ExpiresAt;

        public void ReplaceBy(RefreshTokens newToken)
        {
            Used = true;
            ReplacedByToken = newToken;
        }

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}