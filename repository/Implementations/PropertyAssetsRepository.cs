using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class PropertyAssetsRepository : BaseRepository<PropertyAssets, Guid>, IPropertyAssetsRepository
    {
        public PropertyAssetsRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
