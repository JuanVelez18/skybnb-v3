using application.DTOs;

namespace application.Interfaces
{
    public interface IBookingsApplication
    {
        Task<PageDto<BookingItemDto>> GetBookingsByUserIdAsync(Guid userId, PaginationOptionsDto pagination, BookingFiltersDto? filters);
        Task CreateBooking(BookingsDto bookingDto, Guid userId);
        Task ApproveBooking(Guid bookingId, Guid userId);
    }
}
