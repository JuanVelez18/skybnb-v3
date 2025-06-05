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

        public async Task<PageDto<BookingItemDto>> GetBookingsByUserIdAsync(Guid userId, PaginationOptionsDto paginationDto, BookingFiltersDto? filtersDto)
        {
            var user = await _unitOfWork.Customers.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }

            var filters = filtersDto?.ToDomainBookingFilters();
            var pagination = paginationDto.ToDomainPaginationOptions();
            if (!pagination.IsValid())
            {
                throw new InvalidDataApplicationException("Invalid pagination options.");
            }

            var auditory = new Auditories(
                userId: userId,
                action: "Get Bookings",
                entity: "Bookings",
                entityId: null,
                timestamp: DateTime.UtcNow
            );
            await _unitOfWork.Auditories.AddAsync(auditory);
            await _unitOfWork.CommitAsync();

            var bookingsPage = await _unitOfWork.Bookings.GetBookingsByUserIdAsync(userId, pagination, filters);

            return PageDto<BookingItemDto>.FromDomainPage(bookingsPage, BookingItemDto.FromDomainBooking);
        }

        public async Task CreateBooking(BookingsDto bookingDto, Guid userId)

        {
            if (bookingDto.CheckInDate >= bookingDto.CheckOutDate)
            {
                throw new InvalidDataApplicationException("Check-out date must be after check-in date.");
            }

            var user = await _unitOfWork.Customers.GetByIdAsync(userId);

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
                throw new ConflictApplicationException("You cannot reserve your own property.");
            }
            if (!property.IsActive)
            {
                throw new ConflictApplicationException("The property is not available for booking.");
            }
            if (bookingDto.NumGuests <= 0 || bookingDto.NumGuests > property.MaxGuests)
            {
                throw new InvalidDataApplicationException($"Number of guests must be between 1 and {property.MaxGuests}.");
            }

            var guestPendingBookings = await _unitOfWork.Bookings.GetPendingBookingsByGuestIdAsync(userId);
            var hasPendingBooking = guestPendingBookings.Any(b => b.PropertyId == bookingDto.PropertyId);
            if (hasPendingBooking)
            {
                throw new ConflictApplicationException("You already have a pending booking for this property.");
            }


            var newBooking = new Bookings(
                bookingDto.PropertyId,
                userId,
                bookingDto.CheckInDate,
                bookingDto.CheckOutDate,
                bookingDto.NumGuests,
                bookingDto.Comment
            );
            newBooking.Property = property;
            newBooking.Guest = user;
            newBooking.CalculateTotalPrice();


            var existingConfirmedBookings = await _unitOfWork.Bookings.GetConfirmedBookingsByPropertyIdAsync(bookingDto.PropertyId);
            var hasOverlap = existingConfirmedBookings.Any(newBooking.HasOverlapWith);
            if (hasOverlap)
            {
                throw new ConflictApplicationException("The selected dates overlap with an existing booking.");
            }

            await _unitOfWork.Bookings.AddAsync(newBooking);

            var auditory = new Auditories(
                userId: userId,
                action: "Create Booking",
                entity: "Bookings",
                entityId: newBooking.Id.ToString(),
                timestamp: DateTime.UtcNow
            );

            await _unitOfWork.Auditories.AddAsync(auditory);

            await _unitOfWork.CommitAsync();
        }
    }
}