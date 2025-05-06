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


        public Properties? Property { get; set; }
        public Users? Guest { get; set; }
    }
}
