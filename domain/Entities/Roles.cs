using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Roles(
        string name,
        string? description
        )
    {
        public int Id { get; init; }

        [MaxLength(100)]
        public string Name { get; private set; } = name;

        [MaxLength(255)]
        public string? Description { get; set; } = description;


        public List<Permissions> Permissions { get; set; } = [];
        public List<Users> Users { get; set; } = [];
    }
}
