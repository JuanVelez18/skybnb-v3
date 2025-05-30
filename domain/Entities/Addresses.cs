using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Addresses(
        string street,
        int streetNumber,
        int intersectionNumber,
        int doorNumber,
        string? complement,
        decimal? latitude,
        decimal? longitude
        )
    {
        public Guid Id { get; private set; }

        [MaxLength(100)]
        public string Street { get; set; } = street;

        [Range(1, int.MaxValue)]
        public int StreetNumber { get; set; } = streetNumber;

        [Range(1, int.MaxValue)]
        public int IntersectionNumber { get; set; } = intersectionNumber;

        [Range(1, int.MaxValue)]
        public int DoorNumber { get; set; } = doorNumber;

        [MaxLength(200)]
        public string? Complement { get; set; } = complement;

        [Precision(10, 8)]
        public decimal? Latitude { get; set; } = latitude;

        [Precision(11, 8)]
        public decimal? Longitude { get; set; } = longitude;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Properties? Property { get; set; }
        public Guests? Guest { get; set; }
    }
}
