using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class PropertyTypeRepository: BaseRepository<PropertyTypes, int>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
