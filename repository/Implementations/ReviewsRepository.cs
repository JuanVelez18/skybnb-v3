using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class ReviewsRepository : BaseRepository<Reviews, Guid>, IReviewsRepository
    {
        public ReviewsRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
