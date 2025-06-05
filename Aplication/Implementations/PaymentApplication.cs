using System.Security.Cryptography;
using application.Core;
using application.DTOs;
using application.Interfaces;
using domain.Entities;
using repository.Interfaces;

namespace application.Implementations
{
    public class PaymentApplication : IPaymentApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentApplication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePaymentAsync(Guid userId, PaymentCreationDto paymentCreationDto)
        {
            var user = await _unitOfWork.Customers.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundApplicationException("User not found");
            }

            var booking = await _unitOfWork.Bookings.GetByIdAsync(paymentCreationDto.BookingId);
            if (booking == null)
            {
                throw new NotFoundApplicationException("Booking not found");
            }

            if (booking.Status != BookingStatus.Approved)
            {
                throw new InvalidDataApplicationException("Booking must be approved before making a payment");
            }

            if (booking.GuestId != userId)
            {
                throw new UnauthorizedApplicationException("You are not authorized to make a payment for this booking");
            }

            // Simulate a transaction ID
            using var rnd = RandomNumberGenerator.Create();
            byte[] transactionIdBytes = new byte[16];
            rnd.GetBytes(transactionIdBytes);
            string transactionId = Convert.ToBase64String(transactionIdBytes);

            var payment = new Payments(
                bookingId: booking.Id,
                userId: userId,
                paymentMethod: paymentCreationDto.PaymentMethod!,
                transactionId: transactionId
            );
            booking.Status = BookingStatus.Confirmed;
            payment.Booking = booking;
            payment.User = user;
            payment.CalculateAmountAndFee();
            payment.Status = Payments.PaymentStatus.Completed;
            await _unitOfWork.Payments.AddAsync(payment);

            var auditory = new Auditories(
                userId: userId,
                action: "Create Payment",
                timestamp: DateTime.UtcNow,
                entity: "Payments",
                entityId: payment.Id.ToString()
            );
            await _unitOfWork.Auditories.AddAsync(auditory);

            await _unitOfWork.CommitAsync();
        }
    }
}