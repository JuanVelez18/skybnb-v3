using domain.Entities;

namespace application.DTOs
{
    public class BookingItemDto
    {
        public class BookingPropertyDto
        {
            public required string Title { get; set; }
            public Uri? ImageUrl { get; set; }
            public required string City { get; set; }
            public required string Country { get; set; }
        }

        public class BookingHostDto
        {
            public required Guid Id { get; set; }
            public required string FullName { get; set; }
        }

        public class BookingGuestDto
        {
            public required Guid Id { get; set; }
            public required string FullName { get; set; }
            public required string Email { get; set; }
        }

        public Guid Id { get; set; }
        public BookingPropertyDto? Property { get; set; }
        public BookingHostDto? Host { get; set; }
        public BookingGuestDto? Guest { get; set; }
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public int Guests { get; set; }
        public decimal Total { get; set; }
        public string? Message { get; set; }
        public required string Status { get; set; }
        public decimal? Rating { get; set; }


        static public BookingItemDto FromDomainBooking(Bookings booking)
        {
            return new BookingItemDto
            {
                Id = booking.Id,
                Property = booking.Property != null ? new BookingPropertyDto
                {
                    Title = booking.Property!.Title,
                    ImageUrl = booking.Property!.Multimedia.FirstOrDefault()?.Url,
                    City = booking.Property!.City!.Name,
                    Country = booking.Property!.Country!.Name
                } : null,
                Host = booking.Property != null ? new BookingHostDto
                {
                    Id = booking.Property!.HostId,
                    FullName = $"{booking.Property!.Host!.FirstName} {booking.Property!.Host!.LastName}"
                } : null,
                Guest = booking.Guest != null ? new BookingGuestDto
                {
                    Id = booking.Guest.Id,
                    FullName = $"{booking.Guest.FirstName} {booking.Guest.LastName}",
                    Email = booking.Guest.Email
                } : null,
                CheckIn = booking.CheckInDate,
                CheckOut = booking.CheckOutDate,
                Guests = booking.NumGuests,
                Total = booking.TotalPrice,
                Message = booking.GuestComment,
                Status = ParseStatus(booking.Status),
                Rating = booking.Review?.Rating
            };
        }

        static private string ParseStatus(BookingStatus status)
        {
            return status switch
            {
                BookingStatus.Pending => "pending",
                BookingStatus.Approved => "approved",
                BookingStatus.Confirmed => "confirmed",
                BookingStatus.Cancelled => "cancelled",
                BookingStatus.Completed => "completed",
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}