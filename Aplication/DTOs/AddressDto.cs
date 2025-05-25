using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class AddressDto
    {
        [Required(ErrorMessage = "Street is required")]
        [MinLength(3, ErrorMessage = "Street must be at least 3 characters long")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Street number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Street number must be greater than 0")]
        public int? StreetNumber { get; set; }

        [Required(ErrorMessage = "Intersection number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Intersection number must be greater than 0")]
        public int? IntersectionNumber { get; set; }

        [Required(ErrorMessage = "Door number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Door number must be greater than 0")]
        public int? DoorNumber { get; set; }

        [Required(ErrorMessage = "City ID is required")]
        public int? CityId { get; set; }

        [MaxLength(200, ErrorMessage = "Complement must be at most 200 characters long")]
        public string? Complement { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
