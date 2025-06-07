using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class AddressRepository: BaseRepository<Addresses, Guid>, IAddressRepository
    {
        public AddressRepository(DbConexion conexion): base(conexion)
        {
        }
    }
}
