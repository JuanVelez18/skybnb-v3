using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "The token is required.")]
        [MinLength(36, ErrorMessage = "The token must be at least 36 characters long.")]
        public required string RefreshToken { get; set; }
    }
}
