using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class PropertiesCreationDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(150, ErrorMessage = "Title can't be longer than 150 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(2000, ErrorMessage = "Description must be at most 2000 characters long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Number of bathrooms is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of bathrooms must 0 or greater")]
        public int? NumBathrooms { get; set; }

        [Required(ErrorMessage = "Number of bedrooms is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of bedrooms must 0 or greater")]
        public int? NumBedrooms { get; set; }

        [Required(ErrorMessage = "Number of beds is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of beds must 0 or greater")]
        public int? NumBeds { get; set; }

        [Required(ErrorMessage = "Maximum number of guests is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Maximum number of guests must 0 or greater")]
        public int? MaxGuests { get; set; }

        [Required(ErrorMessage = "Base price per night is required")]
        [Range(0.01, double.MaxValue , ErrorMessage = "Base price per night must be greater than 0")]
        public decimal BasePricePerNight { get; set; }

        [Required(ErrorMessage = "Type ID is required")]
        public int? TypeId { get; set; }

        [Required(ErrorMessage = "Host ID is required")]
        public Guid HostId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public AddressDto? Address { get; set; }
    }
}
