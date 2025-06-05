using domain.Core;
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

        public async Task<Page<Bookings>> GetBookingsByUserIdAsync(Guid userId, PaginationOptions pagination, BookingFilters? filters)
        {
            IQueryable<Bookings> query = _dbSet
                .AsNoTracking()
                .Include(b => b.Property)
                    .ThenInclude(p => p!.Host)
                .Include(b => b.Property)
                    .ThenInclude(p => p!.City)
                .Include(b => b.Property)
                    .ThenInclude(p => p!.Country)
                .Include(b => b.Property)
                    .ThenInclude(p => p!.Multimedia)
                .Include(b => b.Guest)
                .Include(b => b.Review)
                .Where(b => b.GuestId == userId || (b.Property != null && b.Property.HostId == userId));

            if (filters != null)
            {
                foreach (var filter in filters.GetFilterExpressions(userId))
                {
                    query = query.Where(filter);
                }

                var sortExpression = filters.GetSortExpression();
                if (sortExpression != null)
                {
                    query = filters.IsSortDescending()
                        ? query.OrderByDescending(sortExpression)
                        : query.OrderBy(sortExpression);
                }
            }

            long total = await query.CountAsync();
            List<Bookings> bookings = await query
                .Skip((int)((pagination.PageNumber - 1) * pagination.PageSize))
                .Take(pagination.PageSize)
                .ToListAsync();

            return new Page<Bookings>
            {
                Items = bookings,
                TotalCount = total,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
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
