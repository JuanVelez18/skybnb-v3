using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class PaymentRepository : BaseRepository<Payments, Guid>, IPaymentRepository
    {
        public PaymentRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}