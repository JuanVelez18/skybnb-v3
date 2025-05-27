using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Implementations;

namespace ut_presentation.Repositories
{
    [TestClass]
    public class BookingsTestcs
    {
        private DbConexion _conexion;
        private BaseRepository<Bookings, Guid> _repository;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbConexion>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _conexion = new DbConexion(options);
            _repository = new BaseRepository<Bookings, Guid>(_conexion);
        }
        [TestMethod]
        public async Task CreateBooking()
        {
            var booking = new Bookings(
                 propertyId: Guid.NewGuid(),
                 guestId: Guid.NewGuid(),
                 checkInDate: new DateOnly(2025, 12, 24),
                 checkOutDate: new DateOnly(2025, 12, 31),
                 numGuests: 2,
                 totalPrice: 600000
                 );
            await _repository.AddAsync(booking);
            await _conexion.SaveChangesAsync();
        }           
    }
}
