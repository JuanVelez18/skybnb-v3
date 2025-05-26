using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class UserCredentialsDto
    {
        public static class PasswordValidations
        {
            public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";
            public const string PasswordErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character";
        }

        [Required(ErrorMessage = "User email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(20, ErrorMessage = "Password must be at most 20 characters long")]
        [RegularExpression(PasswordValidations.PasswordRegex, ErrorMessage = PasswordValidations.PasswordErrorMessage)]
        public string? Password { get; set; }
    }
}
