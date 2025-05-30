using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Countries(
        string name,
        string isoCode,
        string? phoneCode
        ) : IDisabled
    {
        public int Id { get; private set; }

        [MaxLength(100)]
        public string Name { get; set; } = name;

        [Unicode(false)]
        [MaxLength(2)]
        public string IsoCode { get; set; } = isoCode;

        [Unicode(false)]
        [MaxLength(5)]
        public string? PhoneCode { get; set; } = phoneCode;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public List<Cities> Cities { get; set; } = [];
        public List<Customers> Customers { get; set; } = [];
    }
}
