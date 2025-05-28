using System.Linq.Expressions;
using domain.Entities;

namespace domain.Core
{
    /// <summary>
    /// Provides filtering and sorting capabilities for property searches.
    /// Contains validation logic and expression builders for database queries.
    /// </summary>
    public class PropertyFilters
    {
        /// <summary>
        /// Defines the available sorting options for property results.
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// Sort by price from lowest to highest.
            /// </summary>
            PriceLowToHigh,

            /// <summary>
            /// Sort by price from highest to lowest.
            /// </summary>
            PriceHighToLow,

            /// <summary>
            /// Sort by creation date, newest properties first.
            /// </summary>
            NewestFirst,

            /// <summary>
            /// Sort by average rating from highest to lowest.
            /// </summary>
            RatingHighToLow
        }

        // Properties
        public string? Location { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public decimal? MinPricePerNight { get; set; }
        public decimal? MaxPricePerNight { get; set; }
        public int? MinGuests { get; set; }
        public List<int>? PropertyTypes { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MinBathrooms { get; set; }
        public SortType? SortBy { get; set; }        // Validation
        /// <summary>
        /// Validates all filter criteria to ensure they are logically consistent.
        /// </summary>
        /// <returns>True if all filter criteria are valid; otherwise, false.</returns>
        public bool IsValid()
        {
            return IsValidDateRange() &&
                   IsValidPriceRange() &&
                   IsValidGuestCount();
        }

        /// <summary>
        /// Validates that check-in date is before check-out date when both are specified.
        /// </summary>
        /// <returns>True if date range is valid or not specified; otherwise, false.</returns>
        private bool IsValidDateRange()
        {
            if (CheckInDate.HasValue && CheckOutDate.HasValue)
                return CheckInDate < CheckOutDate;
            return true;
        }

        /// <summary>
        /// Validates that minimum price is not greater than maximum price when both are specified.
        /// </summary>
        /// <returns>True if price range is valid or not specified; otherwise, false.</returns>
        private bool IsValidPriceRange()
        {
            if (MinPricePerNight.HasValue && MaxPricePerNight.HasValue)
                return MinPricePerNight <= MaxPricePerNight;
            return true;
        }

        /// <summary>
        /// Validates that minimum guest count is greater than zero when specified.
        /// </summary>
        /// <returns>True if guest count is valid or not specified; otherwise, false.</returns>
        private bool IsValidGuestCount()
        {
            return !MinGuests.HasValue || MinGuests > 0;
        }

        /// <summary>
        /// Gets all applicable filter expressions based on the current filter criteria.
        /// Only returns expressions for filters that have values specified.
        /// </summary>
        /// <returns>Collection of LINQ expressions that can be applied to filter Properties entities.</returns>
        /// <exception cref="InvalidOperationException">Thrown when filter criteria are invalid.</exception>
        public IEnumerable<Expression<Func<Properties, bool>>> GetFilterExpressions()
        {
            if (!IsValid())
                throw new InvalidOperationException("Invalid filter criteria");

            var expressions = new List<Expression<Func<Properties, bool>>?>
            {
                // Add individual filter expressions
                GetLocationFilter(),
                GetAvailabilityFilter(),
                GetPriceFilter(),
                GetPropertyTypeFilter(),
                GetCapacityFilter()
            };
            expressions.AddRange(GetAmenityFilters());

            return expressions.Where(expr => expr != null).Cast<Expression<Func<Properties, bool>>>();
        }

        /// <summary>
        /// Creates a filter expression for location-based searches.
        /// Matches properties by city name or country name using case-insensitive search.
        /// </summary>
        /// <returns>Expression that filters by location, or null if no location specified.</returns>
        private Expression<Func<Properties, bool>>? GetLocationFilter()
        {
            if (string.IsNullOrWhiteSpace(Location))
                return null;

            return p => (p.Address != null && p.Address.City != null &&
                        p.Address.City.Name != null &&
                        p.Address.City.Name.Contains(Location, StringComparison.OrdinalIgnoreCase)) ||
                       (p.Address != null && p.Address.City != null &&
                        p.Address.City.Country != null &&
                        p.Address.City.Country.Name != null &&
                        p.Address.City.Country.Name.Contains(Location, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Creates filter for property availability based on check-in and check-out dates.
        /// Ensures no overlap with existing bookings.
        /// </summary>
        private Expression<Func<Properties, bool>>? GetAvailabilityFilter()
        {
            if (!CheckInDate.HasValue && !CheckOutDate.HasValue)
                return null;

            // Only check-in date: property available from this date onwards
            if (CheckInDate.HasValue && !CheckOutDate.HasValue)
            {
                return p => p.Bookings.All(booking =>
                    CheckInDate >= booking.CheckOutDate);
            }

            // Only check-out date: property available until this date
            if (!CheckInDate.HasValue && CheckOutDate.HasValue)
            {
                return p => p.Bookings.All(booking =>
                    CheckOutDate <= booking.CheckInDate);
            }

            // Both dates: check for no overlap with requested date range
            return p => p.Bookings.All(booking =>
                CheckOutDate <= booking.CheckInDate ||
                CheckInDate >= booking.CheckOutDate);
        }

        /// <summary>
        /// Creates a filter expression for price range filtering.
        /// Supports minimum price, maximum price, or both price bounds.
        /// </summary>
        /// <returns>Expression that filters by price range, or null if no price filters specified.</returns>
        private Expression<Func<Properties, bool>>? GetPriceFilter()
        {
            // No price filters
            if (!MinPricePerNight.HasValue && !MaxPricePerNight.HasValue)
                return null;

            // Only minimum price
            if (MinPricePerNight.HasValue && !MaxPricePerNight.HasValue)
                return p => p.BasePricePerNight >= MinPricePerNight.Value;

            // Only maximum price  
            if (!MinPricePerNight.HasValue && MaxPricePerNight.HasValue)
                return p => p.BasePricePerNight <= MaxPricePerNight.Value;

            // Both min and max price
            return p => p.BasePricePerNight >= MinPricePerNight!.Value &&
                       p.BasePricePerNight <= MaxPricePerNight!.Value;
        }

        /// <summary>
        /// Creates a filter expression for property type filtering.
        /// Filters properties that match any of the specified property type IDs.
        /// </summary>
        /// <returns>Expression that filters by property types, or null if no types specified.</returns>
        private Expression<Func<Properties, bool>>? GetPropertyTypeFilter()
        {
            if (PropertyTypes?.Any() != true)
                return null;

            return p => PropertyTypes.Contains(p.TypeId);
        }

        /// <summary>
        /// Creates a filter expression for guest capacity filtering.
        /// Ensures properties can accommodate at least the minimum number of guests.
        /// </summary>
        /// <returns>Expression that filters by capacity, or null if no minimum guest count specified.</returns>
        private Expression<Func<Properties, bool>>? GetCapacityFilter()
        {
            if (!MinGuests.HasValue)
                return null;

            return p => p.MaxGuests >= MinGuests.Value;
        }

        /// <summary>
        /// Creates filter expressions for property amenities (bedrooms and bathrooms).
        /// Returns separate expressions for each amenity that has a specified minimum value.
        /// </summary>
        /// <returns>Collection of expressions for amenity filtering. Empty if no amenity filters specified.</returns>
        private List<Expression<Func<Properties, bool>>> GetAmenityFilters()
        {
            var filters = new List<Expression<Func<Properties, bool>>>();

            if (MinBedrooms.HasValue)
                filters.Add(p => p.NumBedrooms >= MinBedrooms.Value);

            if (MinBathrooms.HasValue)
                filters.Add(p => p.NumBathrooms >= MinBathrooms.Value);

            return filters;
        }

        /// <summary>
        /// Gets the LINQ expression for sorting properties based on the specified sort type.
        /// </summary>
        /// <returns>Expression for sorting, or null if no sorting specified or default sort type.</returns>
        public Expression<Func<Properties, object>>? GetSortExpression()
        {
            return SortBy switch
            {
                SortType.PriceLowToHigh => p => p.BasePricePerNight,
                SortType.PriceHighToLow => p => p.BasePricePerNight,
                SortType.NewestFirst => p => p.CreatedAt,
                SortType.RatingHighToLow => p => p.AverageRating ?? 0m,
                _ => null
            };
        }

        /// <summary>
        /// Determines if the current sort type requires descending order.
        /// </summary>
        /// <returns>True if sorting should be in descending order; otherwise, false for ascending order.</returns>
        public bool IsSortDescending()
        {
            return SortBy is SortType.PriceHighToLow or
                            SortType.NewestFirst or
                            SortType.RatingHighToLow;
        }
    }
}