using domain.Entities;
namespace repository.Interfaces
{
    public interface IBookingsRepository : IBaseRepository<Bookings, Guid>
    {
        Task<List<Bookings>> GetConfirmedBookingsByPropertyIdAsync(Guid propertyId);
        Task<List<Bookings>> GetPendingBookingsByGuestIdAsync(Guid guestId);
    }
}
