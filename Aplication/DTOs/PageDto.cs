using domain.Core;

namespace application.DTOs
{
    public class PageDto<T> where T : class
    {
        public required List<T> Results { get; set; }
        public long Page { get; set; }
        public long TotalPages { get; set; }
        public long TotalCount { get; set; }

        static public PageDto<U> FromDomainPage<U>(Page<U> page) where U : class
        {
            return new PageDto<U>
            {
                Results = page.Items,
                Page = page.CurrentPage,
                TotalPages = page.TotalPages,
                TotalCount = page.TotalCount
            };
        }

        static public PageDto<V> FromDomainPage<U, V>(Page<U> page, Func<U, V> converter) where U : class where V : class
        {
            return new PageDto<V>
            {
                Results = page.Items.Select(converter).ToList(),
                Page = page.CurrentPage,
                TotalPages = page.TotalPages,
                TotalCount = page.TotalCount
            };
        }
    }
}