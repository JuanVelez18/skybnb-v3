using System.Linq.Expressions;
using domain.Entities;

namespace domain.Core
{
    public class PropertyFilters
    {
        public enum SortType
        {
            PriceLowToHigh,
            PriceHighToLow,
            NewestFirst,
            RatingHighToLow
        }

        public string? Location { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public decimal? MinPricePerNight { get; set; }
        public decimal? MaxPricePerNight { get; set; }
        public int? MinGuests { get; set; }
        public List<int>? PropertyTypes { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MinBathrooms { get; set; }
        public SortType? SortBy { get; set; }

        public bool IsValid()
        {
            if (CheckInDate.HasValue && CheckOutDate.HasValue && CheckInDate >= CheckOutDate)
                return false;

            if (MinPricePerNight.HasValue && MaxPricePerNight.HasValue && MinPricePerNight > MaxPricePerNight)
                return false;

            if (MinGuests.HasValue && MinGuests <= 0)
                return false;

            return true;
        }

        public bool MatchesProperty(Properties property)
        {
            if (!IsValid())
                throw new InvalidOperationException("Invalid filter criteria");

            return MatchesLocation(property) &&
                   IsAvailableForDates(property) &&
                   MatchesPriceRange(property) &&
                   MatchesPropertyType(property) &&
                   MatchesCapacity(property) &&
                   MatchesAmenities(property);
        }

        private bool MatchesLocation(Properties property)
        {
            if (string.IsNullOrWhiteSpace(Location))
                return true;

            var cityMatch = property.Address?.City?.Name?.Contains(Location, StringComparison.OrdinalIgnoreCase) ?? false;
            var countryMatch = property.Address?.City?.Country?.Name?.Contains(Location, StringComparison.OrdinalIgnoreCase) ?? false;

            return cityMatch || countryMatch;
        }

        private bool IsAvailableForDates(Properties property)
        {
            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
                return true;

            // If there is only a check-in date, check that there are no bookings ending after this date
            if (CheckInDate.HasValue && !CheckInDate.HasValue)
                return property.Bookings.All(booking =>
                    CheckInDate >= booking.CheckOutDate);

            // If there is only a check-out date, check that there are no bookings starting before this date
            if (!CheckInDate.HasValue && CheckOutDate.HasValue)
                return property.Bookings.All(booking =>
                    CheckOutDate <= booking.CheckInDate);


            // If there are both dates, check for overlap with the full range.
            return property.Bookings.All(booking =>
                CheckOutDate <= booking.CheckInDate ||
                CheckInDate >= booking.CheckOutDate);
        }

        private bool MatchesPriceRange(Properties property)
        {
            if (MinPricePerNight.HasValue && property.BasePricePerNight < MinPricePerNight)
                return false;

            if (MaxPricePerNight.HasValue && property.BasePricePerNight > MaxPricePerNight)
                return false;

            return true;
        }

        private bool MatchesPropertyType(Properties property)
        {
            if (PropertyTypes == null || PropertyTypes.Count == 0)
                return true;

            return PropertyTypes.Contains(property.TypeId);
        }

        private bool MatchesCapacity(Properties property)
        {
            if (MinGuests.HasValue && property.MaxGuests < MinGuests)
                return false;

            return true;
        }

        private bool MatchesAmenities(Properties property)
        {
            if (MinBedrooms.HasValue && property.NumBedrooms < MinBedrooms)
                return false;

            if (MinBathrooms.HasValue && property.NumBathrooms < MinBathrooms)
                return false;

            return true;
        }

        public Expression<Func<Properties, object>>? GetSortExpression()
        {
            return SortBy switch
            {
                SortType.PriceLowToHigh => p => p.BasePricePerNight,
                SortType.PriceHighToLow => p => p.BasePricePerNight,
                SortType.NewestFirst => p => p.CreatedAt,
                SortType.RatingHighToLow => p => p.AverageRating ?? 0,
                _ => null
            };
        }

        public bool IsSortDescending()
        {
            return SortBy == SortType.PriceHighToLow ||
                   SortBy == SortType.NewestFirst ||
                   SortBy == SortType.RatingHighToLow;
        }
    }
}
