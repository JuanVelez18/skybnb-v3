using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Bookings(
        Guid propertyId,
        Guid guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numGuests,
        decimal totalPrice,
        string? guestComment
        )
    {
        public Guid Id { get; private set; }

        public Guid PropertyId { get; private set; } = propertyId;

        public Guid GuestId { get; private set; } = guestId;

        public DateOnly CheckInDate { get; private set; } = checkInDate;

        public DateOnly CheckOutDate { get; private set; } = checkOutDate;

        [Range(1, int.MaxValue)]
        public int NumGuests { get; private set; } = numGuests;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Precision(13, 2)]
        public decimal TotalPrice { get; private set; } = totalPrice;

        [MaxLength(500)]
        public string? GuestComment { get; private set; } = guestComment;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public Properties? Property { get; set; }
        public Customers? Guest { get; set; }
        public List<Payments> Payments { get; set; } = [];
        public Reviews? Review { get; set; }
    }
}
