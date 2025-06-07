using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class GuestRepository: BaseRepository<Guests, Guid>, IGuestRepository
    {
        public GuestRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
