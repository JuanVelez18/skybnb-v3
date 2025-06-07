using domain.Core;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class PropertiesRepository : BaseRepository<Properties, Guid>, IPropertiesRepository
    {
        public PropertiesRepository(DbConexion conexion) : base(conexion)
        {
        }

        public async Task<Page<Properties>> GetPropertiesAsync(PaginationOptions pagination, PropertyFilters? filters)
        {
            IQueryable<Properties> query = _dbSet
                .AsNoTracking()
                .Include(p => p.Address)
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Multimedia)
                .Include(p => p.Type)
                .Include(p => p.Reviews);


            if (filters != null && filters.IsValid())
            {
                foreach (var filter in filters.GetFilterExpressions())
                {
                    query = query.Where(filter);
                }

                var sortExpression = filters.GetSortExpression();
                if (sortExpression != null)
                {
                    query = filters.IsSortDescending()
                        ? query.OrderByDescending(sortExpression)
                        : query.OrderBy(sortExpression);
                }
            }

            long total = await query.CountAsync();
            List<Properties> properties = await query
                .Skip((int)((pagination.PageNumber - 1) * pagination.PageSize))
                .Take(pagination.PageSize)
                .ToListAsync();

            return new Page<Properties>
            {
                Items = properties,
                TotalCount = total,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<Properties?> GetDetailByIdAsync(Guid propertyId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Type)
                .Include(p => p.Host)
                .Include(p => p.Address)
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Reviews.OrderByDescending(r => r.CreatedAt).Take(3))
                    .ThenInclude(r => r.Guest)
                .Include(p => p.Multimedia.OrderBy(m => m.Order))
                .FirstOrDefaultAsync(p => p.Id == propertyId);
        }

        public async Task<long> GetReviewsCountAsync(Guid propertyId)
        {
            return await _dbSet
                .Where(p => p.Id == propertyId)
                .SelectMany(p => p.Reviews)
                .LongCountAsync();
        }
    }
}
