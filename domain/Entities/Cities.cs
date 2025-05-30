using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Cities(
        string name,
        int countryId,
        decimal? latitude,
        decimal? longitude
        ): IDisabled
    {
        public int Id { get; private set; }

        [MaxLength(100)]
        public string Name { get; private set; } = name;

        public int CountryId { get; private set; } = countryId;

        [Precision(10, 8)]
        public decimal? Latitude { get; set; } = latitude;

        [Precision(11, 8)]
        public decimal? Longitude { get; set; } = longitude;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public Countries? Country { get; set; }
    }
}
