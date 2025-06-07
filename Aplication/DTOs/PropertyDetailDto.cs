using domain.Entities;

namespace application.DTOs
{
    public class PropertyDetailDto(Properties property, long reviewsCount)
    {
        public class LocationDto
        {
            public required string Address { get; set; }
            public required string City { get; set; }
            public required string Country { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
        }

        public class HostDto
        {
            public required string FullName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class ReviewDto
        {
            public required string GuestFullName { get; set; }
            public decimal Rating { get; set; }
            public required string Comment { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class AssetDto
        {
            public Uri? Url { get; set; }
            public string? Type { get; set; }
        }

        public Guid Id { get; set; } = property.Id;
        public string Title { get; set; } = property.Title;
        public string Description { get; set; } = property.Description;
        public int Guests { get; set; } = property.MaxGuests;
        public int Bedrooms { get; set; } = property.NumBedrooms;
        public int Beds { get; set; } = property.NumBeds;
        public int Bathrooms { get; set; } = property.NumBathrooms;
        public decimal PricePerNight { get; set; } = property.BasePricePerNight;
        public string Type { get; set; } = property.Type!.Name;
        public decimal Rating { get; set; } = property.AverageRating ?? 0;
        public long ReviewsCount { get; set; } = reviewsCount;

        public LocationDto Location { get; set; } = new LocationDto
        {
            Address = $"{property.Address!.Street} {property.Address.StreetNumber} # {property.Address.IntersectionNumber} - {property.Address.DoorNumber}" +
                      $"{(string.IsNullOrEmpty(property.Address.Complement) ? "" : $" - {property.Address.Complement}")}",
            City = property.City!.Name,
            Country = property.Country!.Name,
            Latitude = property.Address.Latitude,
            Longitude = property.Address.Longitude
        };

        public HostDto Host { get; set; } = new HostDto
        {
            FullName = $"{property.Host!.FirstName} {property.Host.LastName}",
            CreatedAt = property.Host.CreatedAt
        };

        public List<ReviewDto> LastReviews { get; set; } = [.. property.Reviews.Select(r => new ReviewDto
        {
            GuestFullName = $"{r.Guest!.FirstName} {r.Guest.LastName}",
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        })];

        public List<AssetDto> Multimedia { get; set; } = [.. property.Multimedia.Select(m => new AssetDto
        {
            Url = m.Url,
            Type = m.Type
        })];
    }
}