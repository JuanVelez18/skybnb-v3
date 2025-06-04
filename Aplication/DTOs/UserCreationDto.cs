using System.ComponentModel.DataAnnotations;
using static application.DTOs.UserCredentialsDto;

namespace application.DTOs
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "Dni is required")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(20, ErrorMessage = "Password must be at most 20 characters long")]
        [RegularExpression(PasswordValidations.PasswordRegex, ErrorMessage = PasswordValidations.PasswordErrorMessage)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Birthday is required")]
        public DateOnly? Birthday { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int? Country { get; set; }

        [MinLength(9, ErrorMessage = "Phone must be at least 8 characters long")]
        [MaxLength(20, ErrorMessage = "Phone must be at most 20 characters long")]
        [RegularExpression(@"^\d{9,20}$", ErrorMessage = "The phone must contain only numbers")]
        public string? Phone { get; set; }

        public UserCredentialsDto ToCredentials() => new()
        {
            Email = Email,
            Password = Password
        };
    }
}
