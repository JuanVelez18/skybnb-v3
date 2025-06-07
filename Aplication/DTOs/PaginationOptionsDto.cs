using System.ComponentModel.DataAnnotations;
using domain.Core;

namespace application.DTOs
{
    public class PaginationOptionsDto
    {
        public long? Page { get; set; }
        public int? PageSize { get; set; }

        public PaginationOptions ToDomainPaginationOptions()
        {
            return new PaginationOptions
            {
                PageNumber = Page ?? PaginationOptions.MinPageNumber,
                PageSize = PageSize ?? PaginationOptions.MinPageSize
            };
        }
    }
}
