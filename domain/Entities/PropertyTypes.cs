using System.ComponentModel.DataAnnotations;

namespace domain.Entities
{
    public class PropertyTypes(
        string name,
        string description
        ) : IDisabled
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = name;

        public string Description { get; set; } = description;

        public bool IsActive { get; set; } = true;


        public List<Properties> Properties { get; set; } = [];
    }
}
