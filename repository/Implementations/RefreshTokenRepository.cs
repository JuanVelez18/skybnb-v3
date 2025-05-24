using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class RefreshTokenRepository: BaseRepository<RefreshTokens, long>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DbConexion conexion): base(conexion)
        {
        }
    }
}
