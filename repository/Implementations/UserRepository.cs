using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UserRepository : BaseRepository<Users, Guid>, IUserRepository
    {
        public UserRepository(DbConexion conexion) : base(conexion)
        {
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
        }
    }
}
