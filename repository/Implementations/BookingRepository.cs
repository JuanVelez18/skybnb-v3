using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class BookingRepository : BaseRepository<Bookings, Guid>, IBookingsRepository
    {
        public BookingRepository(DbConexion conexion) : base(conexion)
        {
        }

        public async Task<List<Bookings>> GetConfirmedBookingsByPropertyIdAsync(Guid propertyId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(b => b.PropertyId == propertyId && b.Status == BookingStatus.Confirmed)
                .ToListAsync();
        }

        public async Task<List<Bookings>> GetPendingBookingsByGuestIdAsync(Guid guestId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(b => b.GuestId == guestId && b.Status == BookingStatus.Pending)
                .ToListAsync();
        }
    }
}
