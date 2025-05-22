using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbConexion _conexion;
        public IUserRepository Users { get; private set; }
        public IGuestRepository Guests { get; private set; }
        public IAddressRepository Addresses { get; private set; }

        public UnitOfWork(DbConexion conexion)
        {
            _conexion = conexion;
            Users = new UserRepository(_conexion);
            Guests = new GuestRepository(_conexion);
            Addresses = new AddressRepository(_conexion);
        }

        public async Task<int> CommitAsync()
        {
            return await _conexion.SaveChangesAsync();
        }

        public void Dispose()
        {
            _conexion.Dispose();
        }
    }
}
