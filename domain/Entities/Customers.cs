using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Dni), IsUnique = true)]
    public class Customers(
        string dni,
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        DateOnly birthday,
        int countryId,
        string? phone
        ) : Users(email, passwordHash)
    {
        [Unicode(false)]
        [MaxLength(20)]
        public string Dni { get; set; } = dni;

        [MaxLength(50)]
        public string FirstName { get; set; } = firstName;

        [MaxLength(50)]
        public string LastName { get; set; } = lastName;

        public DateOnly Birthday { get; set; } = birthday;

        public int CountryId { get; set; } = countryId;

        [Unicode(false)]
        [MaxLength(20)]
        public string? Phone { get; set; } = phone;


        public Countries? Country { get; set; }
        public Guests? GuestProfile { get; set; }
        public List<Properties> HostedProperties { get; set; } = [];
        public List<Bookings> Bookings { get; set; } = [];
        public List<Payments> Payments { get; set; } = [];
        public List<Reviews> ReviewsWritten { get; set; } = [];
    }
}
