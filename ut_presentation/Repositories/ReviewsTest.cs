using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Implementations;

namespace ut_presentation.Repositories
{
    [TestClass]
    public class ReviewsTestcs
    {
        private DbConexion _conexion;
        private BaseRepository<Reviews, Guid> _repository;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbConexion>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _conexion = new DbConexion(options);
            _repository = new BaseRepository<Reviews, Guid>(_conexion);
        }
        [TestMethod]
        public async Task CreateReviews()
        {
            var review = new Reviews(
                 bookingId: Guid.NewGuid(),
                 propertyId: Guid.NewGuid(),
                 guestId: Guid.NewGuid(),
                 rating: 4.2m,
                 comment: "Very Nice"
                 );
            await _repository.AddAsync(review);
            await _conexion.SaveChangesAsync();
        }
    }
}

