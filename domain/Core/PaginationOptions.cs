namespace domain.Core
{
    public class PaginationOptions
    {
        public long PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public static readonly long MinPageNumber = 1;
        public static readonly int MinPageSize = 1;
        public static readonly int MaxPageSize = 50;

        public bool IsValid()
        {
            return PageNumber >= MinPageNumber &&
                   PageSize >= MinPageSize &&
                   PageSize <= MaxPageSize;
        }
    }
}
