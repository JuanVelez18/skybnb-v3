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
        decimal totalPrice
        ): AuditableEntity
    {
        public Guid Id { get; }

        public Guid PropertyId { get; } = propertyId;

        public Guid GuestId { get; } = guestId;

        public DateOnly CheckInDate { get; } = checkInDate;

        public DateOnly CheckOutDate { get; } = checkOutDate;

        [Range(1, int.MaxValue)]
        public int NumGuests { get; } = numGuests;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Precision(13, 2)]
        public decimal TotalPrice { get; } = totalPrice;


        public Properties? Property { get; }
        public Users? Guest { get; }
    }
}
