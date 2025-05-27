using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class PropertyTypesRepository : BaseRepository<PropertyTypes, int>, IPropertyTypesRepository
    {
        public PropertyTypesRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
