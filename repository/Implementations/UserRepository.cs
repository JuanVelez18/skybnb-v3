using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UserRepository : BaseRepository<Users, Guid>, IUserRepository
    {
        public UserRepository(DbConexion conexion) : base(conexion) { }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public override async Task<Users?> GetByIdAsync(Guid id)
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

        public void AssignRole(Users user, Roles role)
        {
            user.Roles!.Add(role);
        }
    }
}
