using domain.Entities;

namespace repository.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payments, Guid>
    { }
}