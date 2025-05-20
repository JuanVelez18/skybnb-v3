using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class GuestCreationDto: UserCreationDto
    {
        [Required(ErrorMessage = "Address is required")]
        public required AddressDto Address { get; set; }
    }
}
