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
        ) : AuditableEntity
    {
        public Guid Id { get; }

        public Guid BookingId { get; } = bookingId;

        public Guid PropertyId { get; } = propertyId;

        public Guid GuestId { get; } = guestId;

        [Range(1,5)]
        public decimal Rating { get; } = rating;

        [MaxLength(2000)]
        public string Comment { get; } = comment;


        public bool IsActive { get; set; } = true;

        public Bookings? Booking { get; set; }
        public Properties? Property { get; set; }
        public Users? Guest { get; set; }
    }
}
