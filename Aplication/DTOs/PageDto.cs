using domain.Core;

namespace application.DTOs
{
    public class PageDto<T> where T : class
    {
        public required List<T> Results { get; set; }
        public long Page { get; set; }
        public long TotalPages { get; set; }
        public long TotalCount { get; set; }

        static public PageDto<T> FromDomainPage(Page<T> page)
        {
            return new PageDto<T>
            {
                Results = page.Items,
                Page = page.CurrentPage,
                TotalPages = page.TotalPages,
                TotalCount = page.TotalCount
            };
        }

        static public PageDto<T> FromDomainPage<U>(Page<U> page, Func<U, T> converter) where U : class
        {
            return new PageDto<T>
            {
                Results = page.Items.Select(converter).ToList(),
                Page = page.CurrentPage,
                TotalPages = page.TotalPages,
                TotalCount = page.TotalCount
            };
        }
    }
}