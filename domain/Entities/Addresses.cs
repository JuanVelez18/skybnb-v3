using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Addresses(
        string street,
        int streetNumber,
        int intersectionNumber,
        int doorNumber,
        int cityId,
        int countryId,
        string? complement,
        decimal? latitude,
        decimal? longitude
        ): AuditableEntity
    {
        public Guid Id { get; }

        [MaxLength(100)]
        public string Street { get; set; } = street;

        [Range(1, int.MaxValue)]
        public int StreetNumber { get; set; } = streetNumber;

        [Range(1, int.MaxValue)]
        public int IntersectionNumber { get; set; } = intersectionNumber;

        [Range(1, int.MaxValue)]
        public int DoorNumber { get; set; } = doorNumber;

        public int CityId { get; set; } = cityId;

        public int CountryId { get; set; } = countryId;

        [MaxLength(200)]
        public string? Complement { get; set; } = complement;

        [Precision(10, 8)]
        public decimal? Latitude { get; set; } = latitude;

        [Precision(11, 8)]
        public decimal? Longitude { get; set; } = longitude;

        public bool IsActive { get; set; } = true;


        public Cities? City { get; set; }

        public Countries? Country { get; set; }
    }
}
