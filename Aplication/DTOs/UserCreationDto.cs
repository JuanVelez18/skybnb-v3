using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class UserCreationDto
    {
        public required string Dni { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        [Required(ErrorMessage = "Birthday is required")]
        public required DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "CountryId is required")]
        public required int CountryId { get; set; }
        public string? Phone { get; set; }

        public UserCredentialsDto ToCredentials() => new()
        {
            Email = Email,
            Password = Password
        };
    }
}
