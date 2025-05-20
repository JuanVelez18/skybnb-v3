using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UserRepository: BaseRepository<Users, Guid>, IUserRepository
    {
        public UserRepository(DbConexion conexion) : base(conexion) {}

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AssignRole(Guid userId, int roleId)
        {
            var user = await _dbSet
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var role = await _conexion.Roles.FindAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {roleId} not found");


            var alreadyHasRole = user.Roles.Any(r => r.Id == roleId);
            if (alreadyHasRole) return;

            user.Roles.Add(role);
            user.UpdatedAt = DateTime.UtcNow;
        }
    }
}
