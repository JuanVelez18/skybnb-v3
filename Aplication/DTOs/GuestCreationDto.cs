using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class GuestCreationDto : UserCreationDto
    {
        [Required(ErrorMessage = "Address is required")]
        public AddressDto? Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Range(1, int.MaxValue, ErrorMessage = "City must be greater than 0")]
        public int? City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Country must be greater than 0")]
        public int? ResidenceCountry { get; set; }
    }
}
