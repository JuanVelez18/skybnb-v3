using domain.Core;
using domain.Entities;

namespace repository.Interfaces
{
    public interface IPropertiesRepository : IBaseRepository<Properties, Guid>
    {
        Task<Page<Properties>> GetPropertiesAsync(PaginationOptions pagination, PropertyFilters? filters);
        Task<Properties?> GetDetailByIdAsync(Guid propertyId);
        Task<long> GetReviewsCountAsync(Guid propertyId);
    }
}
