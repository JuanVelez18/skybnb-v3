using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    public class Payments(
        Guid bookingId,
        Guid userId,
        decimal amount,
        decimal fee,
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

        public Guid UserId { get; private set; } = userId;

        [Precision(13, 2)]
        public decimal Amount { get; private set; } = amount;

        [Precision(13, 2)]
        public decimal Fee { get; private set; } = fee;

        [MaxLength(100)]
        public string PaymentMethod { get; private set; } = paymentMethod;

        public string? TransactionId { get; private set; } = transactionId;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;


        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Bookings? Booking { get; set; }
        public Customers? User { get; set; }
    }
}
