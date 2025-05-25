using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class ReviewsDto
    {
        [Required(ErrorMessage = "Booking ID is required")]
        public Guid BookingId { get; set; }

        [Required(ErrorMessage = "Property ID is required")]
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "Guest ID is required")]
        public Guid GuestId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1 and 5")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [MaxLength(2000, ErrorMessage = "Comment must be at most 2000 characters long")]
        public string Comment { get; set; }
    }
}
