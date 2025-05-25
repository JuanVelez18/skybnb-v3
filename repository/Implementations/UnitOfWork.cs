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
        public IRoleRepository Roles { get; private set; }
        public IPropertiesRepository Properties { get; private set; }
        public IReviewsRepository Reviews { get; private set; }
        public IBookingsRepository Bookings { get; private set; }
        public IAuditoryRepository Auditories { get; private set; }

        public UnitOfWork(DbConexion conexion)
        {
            _conexion = conexion;
            Users = new UserRepository(_conexion);
            Guests = new GuestRepository(_conexion);
            Addresses = new AddressRepository(_conexion);
            Roles = new RoleRepository(_conexion);
            Properties = new PropertiesRepository(_conexion);
            Reviews = new ReviewsRepository(_conexion);
            Bookings = new BookingRepository(_conexion);
            Auditories = new AuditoryRepository(_conexion);
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
