using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class PropertiesRepository : BaseRepository<Properties, Guid>, IPropertiesRepository
    {
        public PropertiesRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
