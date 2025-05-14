using System.ComponentModel.DataAnnotations;

namespace asp_services.Dtos
{
    public class UserDto
    {
        public string Dni { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Birthday is required")]
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "CountryId is required")]
        public int CountryId { get; set; }
        public string? Phone { get; set; }
    }
}
