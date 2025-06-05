using System.Linq.Expressions;
using domain.Entities;

namespace domain.Core
{
    public class BookingFilters
    {
        public enum SortType
        {
            NewestFirst,
            OldestFirst,
            CheckInDateNearest,
            TotalPriceAsc,
        }

        public enum RoleType
        {
            Guest,
            Host
        }

        public RoleType? Role { get; set; }
        public BookingStatus? Status { get; set; }
        public SortType? SortBy { get; set; }


        public List<Expression<Func<Bookings, bool>>> GetFilterExpressions(Guid userId)
        {
            var expressions = new List<Expression<Func<Bookings, bool>>?>
            {
                GetRoleFilter(userId),
                GetStatusFilter()
            };

            return [.. expressions.Where(e => e != null)];
        }

        private Expression<Func<Bookings, bool>>? GetRoleFilter(Guid userId)
        {
            if (!Role.HasValue) return null;

            if (Role == RoleType.Guest) return b => b.GuestId == userId;

            if (Role == RoleType.Host) return b => b.Property!.HostId == userId;

            return null;
        }

        private Expression<Func<Bookings, bool>>? GetStatusFilter()
        {
            if (!Status.HasValue) return null;

            return b => b.Status == Status.Value;
        }

        public Expression<Func<Bookings, object>>? GetSortExpression()
        {
            if (!SortBy.HasValue) return null;

            return SortBy switch
            {
                SortType.NewestFirst => b => b.CreatedAt,
                SortType.OldestFirst => b => b.CreatedAt,
                SortType.CheckInDateNearest => b => b.CheckInDate,
                SortType.TotalPriceAsc => b => b.TotalPrice,
                _ => null
            };
        }

        public bool IsSortDescending()
        {
            return SortBy is SortType.NewestFirst or SortType.CheckInDateNearest;
        }
    }
}