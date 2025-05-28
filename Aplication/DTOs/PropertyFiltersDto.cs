using domain.Core;

namespace application.DTOs
{
    public class PropertyFiltersDto
    {
        public string? Location { get; set; }
        public DateOnly? CheckIn { get; set; }
        public DateOnly? CheckOut { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Guests { get; set; }
        public List<int>? Types { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string? SortBy { get; set; }

        public PropertyFilters ToDomainPropertyFilters()
        {
            return new PropertyFilters
            {
                Location = Location,
                CheckInDate = CheckIn,
                CheckOutDate = CheckOut,
                MinPricePerNight = MinPrice,
                MaxPricePerNight = MaxPrice,
                MinGuests = Guests,
                PropertyTypes = Types,
                MinBedrooms = Bedrooms,
                MinBathrooms = Bathrooms,
                SortBy = ParseSortType(SortBy)
            };
        }

        static private PropertyFilters.SortType? ParseSortType(string? sortBy)
        {
            return sortBy switch
            {
                "price-low" => PropertyFilters.SortType.PriceLowToHigh,
                "price-high" => PropertyFilters.SortType.PriceHighToLow,
                "newest" => PropertyFilters.SortType.NewestFirst,
                "rating" => PropertyFilters.SortType.RatingHighToLow,
                _ => null
            };
        }
    }
}