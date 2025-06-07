using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class BookingsDto
    {
        [Required(ErrorMessage = "Property ID is required")]
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "Check-In Date is required")]
        public DateOnly CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out Date is required")]
        public DateOnly CheckOutDate { get; set; }

        [Required(ErrorMessage = "Maximum number of guests is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be greater than 0")]
        public int NumGuests { get; set; }

        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }
    }
}
