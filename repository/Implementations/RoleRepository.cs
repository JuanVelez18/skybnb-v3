using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class RoleRepository: BaseRepository<Roles, int>, IRoleRepository
    {
        public RoleRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
