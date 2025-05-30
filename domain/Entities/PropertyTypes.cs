using System.ComponentModel.DataAnnotations;

namespace domain.Entities
{
    public class PropertyTypes(
        string name,
        string description
        )
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = name;

        public string Description { get; set; } = description;


        public List<Properties> Properties { get; set; } = [];
    }
}
