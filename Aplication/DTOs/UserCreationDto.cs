using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class UserCreationDto
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
