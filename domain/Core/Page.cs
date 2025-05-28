namespace domain.Core
{
    public class Page<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public required long TotalCount { get; set; }
        public required int CurrentPage { get; set; }
        public required int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
