using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Permissions(
        string name,
        string? description
        )
    {
        public int Id { get; init; }

        [MaxLength(100)]
        public string Name { get; } = name;

        [MaxLength(255)]
        public string? Description { get; set; } = description;


        public List<Roles> Roles { get; } = [];
    }
}
