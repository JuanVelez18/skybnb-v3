using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Bookings(
        Guid? propertyId,
        Guid? guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numGuests,
        string? guestComment
        )
    {
        public static readonly decimal FEE_PERCENTAGE = 0.1m; // 5% fee on total price

        public Guid Id { get; private set; }

        public Guid? PropertyId { get; private set; } = propertyId;

        public Guid? GuestId { get; private set; } = guestId;

        public DateOnly CheckInDate { get; private set; } = checkInDate;

        public DateOnly CheckOutDate { get; private set; } = checkOutDate;

        [Range(1, int.MaxValue)]
        public int NumGuests { get; private set; } = numGuests;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Precision(13, 2)]
        public decimal TotalPrice { get; private set; }

        [MaxLength(500)]
        public string? GuestComment { get; private set; } = guestComment;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public Properties? Property { get; set; }
        public Customers? Guest { get; set; }
        public List<Payments> Payments { get; set; } = [];
        public Reviews? Review { get; set; }


        public void CalculateTotalPrice()
        {
            if (Property == null)
            {
                throw new InvalidOperationException("Property must be set before calculating total price");
            }

            var nights = (CheckOutDate.ToDateTime(new TimeOnly()) - CheckInDate.ToDateTime(new TimeOnly())).TotalDays;
            if (nights <= 0)
            {
                throw new InvalidOperationException("Check-out date must be after check-in date");
            }

            TotalPrice = Property.BasePricePerNight * (decimal)nights;
            TotalPrice += TotalPrice * FEE_PERCENTAGE; // Add fee   
        }

        public bool HasOverlapWith(Bookings otherBooking)
        {
            return CheckInDate < otherBooking.CheckOutDate &&
                   CheckOutDate > otherBooking.CheckInDate &&
                   PropertyId == otherBooking.PropertyId;
        }
    }
}
