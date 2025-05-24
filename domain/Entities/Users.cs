using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Dni), IsUnique = true)]
    public class Users(
        string dni,
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        DateOnly birthday,
        int countryId,
        string? phone
        ): ISoftDeletable
    {
        public Guid Id { get; private set; }

        [Unicode(false)]
        [MaxLength(20)]
        public string Dni { get; set; } = dni;

        [MaxLength(50)]
        public string FirstName { get; set; } = firstName;

        [MaxLength(50)]
        public string LastName { get; set; } = lastName;

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = email;

        [Unicode(false)]
        [MaxLength(120)]
        public string PasswordHash { get; set; } = passwordHash;

        public DateOnly Birthday { get; set; } = birthday;

        public int CountryId { get; set; } = countryId;

        [Unicode(false)]
        [MaxLength(20)]
        public string? Phone { get; set; } = phone;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public Countries? Country { get; set; }
        public Guests? Guest { get; set; }
        public List<Roles> Roles { get; set; } = [];
        public List<Properties> HostedProperties { get; set; } = [];
        public List<Bookings> Bookings { get; set; } = [];
        public List<Reviews> ReviewsWritten { get; set; } = [];
    }
}
