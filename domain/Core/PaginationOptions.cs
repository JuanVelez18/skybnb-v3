namespace domain.Core
{
    public class PaginationOptions
    {
        public int PageNumber { get; set; } = 1;
        private int _PageSize { get; set; } = 10;
        private const int MaxPageSize = 50;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
