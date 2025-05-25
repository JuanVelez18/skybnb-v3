using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class CountryRepository: BaseRepository<Countries, int>, ICountryRepository
    {
        public CountryRepository(DbConexion context) : base(context)
        {
        }
    }
}
