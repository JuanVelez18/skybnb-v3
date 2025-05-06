using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(BookingId), IsUnique = true)]
    public class Reviews(
        Guid bookingId,
        Guid propertyId,
        Guid guestId,
        decimal rating,
        string comment
        )
    {
        public Guid Id { get; private set; }

        public Guid BookingId { get; private set; } = bookingId;

        public Guid PropertyId { get; private set; } = propertyId;

        public Guid GuestId { get; private set; } = guestId;

        [Range(1,5)]
        [Precision(2, 1)]
        public decimal Rating { get; private set; } = rating;

        [MaxLength(2000)]
        public string Comment { get; private set; } = comment;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public Bookings? Booking { get; set; }
        public Properties? Property { get; set; }
        public Users? Guest { get; set; }
    }
}
