using domain.Entities;

namespace application.DTOs
{
    public class PropertySummaryDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public int MaxGuests { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal Price { get; set; }
        public required string PropertyType { get; set; }
        public decimal Rating { get; set; }
        public int Reviews { get; set; }

        static public PropertySummaryDto FromDomainProperty(Properties property)
        {
            return new PropertySummaryDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                PhotoUrl = property.Multimedia.First()?.Url?.ToString(),
                City = property.City!.Name,
                Country = property.Country!.Name,
                MaxGuests = property.MaxGuests,
                Bedrooms = property.NumBedrooms,
                Bathrooms = property.NumBathrooms,
                Price = property.BasePricePerNight,
                PropertyType = property.Type!.Name,
                Rating = property.AverageRating ?? 0,
                Reviews = property.Reviews.Count
            };
        }
    }
}