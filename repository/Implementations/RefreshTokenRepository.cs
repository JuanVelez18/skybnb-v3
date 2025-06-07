using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class RefreshTokenRepository : BaseRepository<RefreshTokens, long>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DbConexion conexion) : base(conexion)
        {
        }

        public async Task<RefreshTokens?> GetByHashedTokenAsync(string hashedToken)
        {
            return await _dbSet
                .Include(rt => rt.User)
                .Include(rt => rt.ReplacedByToken)
                .Include(rt => rt.ReplacesTokens)
                .FirstOrDefaultAsync(rt => rt.TokenValue == hashedToken);
        }
    }
}
