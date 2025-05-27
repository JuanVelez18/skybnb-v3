using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Implementations;

namespace ut_presentation.Repositories
{
    [TestClass]
    public class PropertyTest
    {
        private DbConexion _conexion;
        private BaseRepository<Properties, Guid> _repository;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbConexion>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _conexion = new DbConexion(options);
            _repository = new BaseRepository<Properties, Guid>(_conexion);
        }
        [TestMethod]
        public async Task CreateProperty()
        {

            var newAddress = new Addresses(
                street: "Calle",
                streetNumber: 41,
                intersectionNumber: 27,
                doorNumber: 32,
                cityId: 2,
                complement: "Apto 302" ,
                latitude: 0.1m,
                longitude: 0.2m
            );
            var property = new Properties(
                title: "Casa Bonita",
                description: "Descripción de prueba",
                numBathrooms: 2,
                numBedrooms: 3,
                numBeds: 3,
                maxGuests: 5,
                basePricePerNight: 150000,
                typeId: 1,
                hostId: Guid.NewGuid(),
                addressId: newAddress.Id
            );

            await _repository.AddAsync(property);
            await _conexion.SaveChangesAsync();

        }
        [TestCleanup]
        public void TearDown()
        {
            _conexion.Dispose();
        }
    }
}
