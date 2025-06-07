using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class CityRepository : BaseRepository<Cities, int>, ICityRepository
    {
        public CityRepository(DbConexion context) : base(context)
        {
        }
    }
}
