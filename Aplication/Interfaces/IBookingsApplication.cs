using application.DTOs;

namespace application.Interfaces
{
    public interface IBookingsApplication
    {
        Task CreateBooking(BookingsDto bookingDto, Guid userId);
    }
}
