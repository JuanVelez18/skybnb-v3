using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Countries(
        string name,
        string isoCode,
        string? phoneCode
        ): AuditableEntity
    {
        public int Id { get; }

        [MaxLength(100)]
        public string Name { get; set; } = name;

        [Unicode(false)]
        [MaxLength(2)]
        public string IsoCode { get; set; } = isoCode;

        [Unicode(false)]
        [MaxLength(5)]
        public string? PhoneCode { get; set; } = phoneCode;

        public bool IsActive { get; set; } = true;
    }
}
