using System.ComponentModel.DataAnnotations;

namespace application.DTOs
{
    public class PaymentCreationDto
    {
        public Guid BookingId { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [MinLength(5, ErrorMessage = "Payment method must be at least 3 characters long")]
        public string? PaymentMethod { get; set; }
    }
}