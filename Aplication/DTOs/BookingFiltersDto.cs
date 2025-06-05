using domain.Core;
using domain.Entities;

namespace application.DTOs
{
    public class BookingFiltersDto
    {
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? SortBy { get; set; }


        public BookingFilters ToDomainBookingFilters()
        {
            return new BookingFilters
            {
                Role = ParseRole(Role),
                Status = ParseBookingStatus(Status),
                SortBy = ParseSortType(SortBy)
            };
        }

        static private BookingFilters.RoleType? ParseRole(string? role)
        {
            return role switch
            {
                "guest" => BookingFilters.RoleType.Guest,
                "host" => BookingFilters.RoleType.Host,
                _ => null
            };
        }

        static private BookingStatus? ParseBookingStatus(string? status)
        {
            return status switch
            {
                "pending" => BookingStatus.Pending,
                "approved" => BookingStatus.Approved,
                "confirmed" => BookingStatus.Confirmed,
                "cancelled" => BookingStatus.Cancelled,
                "completed" => BookingStatus.Completed,
                _ => null
            };
        }

        static private BookingFilters.SortType? ParseSortType(string? sortBy)
        {
            return sortBy switch
            {
                "newest" => BookingFilters.SortType.NewestFirst,
                "oldest" => BookingFilters.SortType.OldestFirst,
                "checkin" => BookingFilters.SortType.CheckInDateNearest,
                "amount" => BookingFilters.SortType.TotalPriceAsc,
                _ => BookingFilters.SortType.NewestFirst
            };
        }
    }
}