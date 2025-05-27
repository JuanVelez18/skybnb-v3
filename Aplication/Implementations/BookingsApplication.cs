using application.Core;
using application.DTOs;
using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using repository.Interfaces;
using System.Text.Json;

namespace application.Implementations
{
    public class BookingsApplication : IBookingsApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingsApplication(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateBooking(BookingsDto bookingDto, Guid userId)

        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }

            var property = await _unitOfWork.Properties.GetByIdAsync(bookingDto.PropertyId);
            if (property == null)
            {
                throw new NotFoundApplicationException("Property not found.");
            }
            if (property.HostId == userId) 
            {
                throw new InvalidOperationException("You cannot reserve your own property.");
            }

            var newBooking = new Bookings(
                bookingDto.PropertyId,
                userId,
                bookingDto.CheckInDate,
                bookingDto.CheckOutDate,
                bookingDto.NumGuests,
                bookingDto.TotalPrice
        );
            newBooking.Property = property;
            newBooking.Guest = user;

            await _unitOfWork.Bookings.AddAsync(newBooking);



            var auditory = new Auditories(
                userId: userId,
                action: "Create Booking",
                entity: "Bookings",
                entityId: newBooking.Id.ToString(),
                details: JsonSerializer.Serialize(newBooking, new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                }),
                timestamp: DateTime.UtcNow
            );

            await _unitOfWork.Auditories.AddAsync(auditory);

            await _unitOfWork.CommitAsync();

        }
    }
}