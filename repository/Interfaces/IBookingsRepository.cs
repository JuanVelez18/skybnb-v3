using domain.Core;
using domain.Entities;

namespace repository.Interfaces
{
    public interface IBookingsRepository : IBaseRepository<Bookings, Guid>
    {
        Task<Page<Bookings>> GetBookingsByUserIdAsync(Guid userId, PaginationOptions pagination, BookingFilters? filters);
        Task<List<Bookings>> GetConfirmedBookingsByPropertyIdAsync(Guid propertyId);
        Task<List<Bookings>> GetPendingBookingsByGuestIdAsync(Guid guestId);
    }
}
