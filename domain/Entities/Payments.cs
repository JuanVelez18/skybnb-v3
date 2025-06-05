using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Payments(
        Guid bookingId,
        Guid? userId,
        string paymentMethod,
        string? transactionId
    )
    {
        public enum PaymentStatus
        {
            Pending,
            Completed,
            Failed,
            Refunded
        }

        public Guid Id { get; private set; }

        public Guid BookingId { get; private set; } = bookingId;

        public Guid? UserId { get; private set; } = userId;

        [Precision(13, 2)]
        public decimal Amount { get; private set; }

        [Precision(13, 2)]
        public decimal Fee { get; private set; }

        [MaxLength(100)]
        public string PaymentMethod { get; private set; } = paymentMethod;

        public string? TransactionId { get; private set; } = transactionId;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;


        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Bookings? Booking { get; set; }
        public Customers? User { get; set; }


        public void CalculateAmountAndFee()
        {
            if (Booking == null)
            {
                throw new ArgumentNullException(nameof(Booking), "Booking cannot be null");
            }

            var rawAmount = Booking.TotalPrice / (1 + Bookings.FEE_PERCENTAGE);

            Amount = rawAmount * (1 + Bookings.FEE_PERCENTAGE);
            Fee = rawAmount * Bookings.FEE_PERCENTAGE;
        }
    }
}
