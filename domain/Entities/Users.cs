using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class Users(
        string email,
        string passwordHash
    ) : IDisabled
    {
        public Guid Id { get; private set; }

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; private set; } = email;

        [Unicode(false)]
        [MaxLength(120)]
        public string PasswordHash { get; set; } = passwordHash;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<Roles> Roles { get; set; } = [];
        public List<RefreshTokens> RefreshTokens { get; set; } = [];
    }
}
