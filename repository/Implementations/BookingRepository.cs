using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class BookingRepository : BaseRepository<Bookings, Guid>, IBookingsRepository
    {
        public BookingRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
