using domain.Entities;

namespace repository.Interfaces
{
    public interface IUserRepository : IBaseRepository<Users, Guid>
    {
        Task<Users?> GetByEmailAsync(string email);
        void AssignRole(Users user, Roles role);
    }
}
