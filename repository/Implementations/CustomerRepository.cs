using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class CustomerRepository : BaseRepository<Customers, Guid>, ICustomerRepository
    {
        public CustomerRepository(DbConexion conexion) : base(conexion) { }

        public async Task<Customers?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public override async Task<Customers?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<List<Permissions>> GetUserPermissionsAsync(Guid userId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(user => user.Id == userId)
                .SelectMany(user => user.Roles)
                .SelectMany(role => role.Permissions)
                .Distinct()
                .ToListAsync();
        }

        public void AssignRole(Customers user, Roles role)
        {
            user.Roles!.Add(role);
        }
    }
}
