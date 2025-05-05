using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Roles(
        string name,
        string? description
        ): AuditableEntity
    {
        public int Id { get; }

        [MaxLength(100)]
        public string Name { get; } = name;

        [MaxLength(255)]
        public string? Description { get; set; } = description;


        public List<Permissions> Permissions { get; } = [];
        public List<Users> Users { get; } = [];
    }
}
