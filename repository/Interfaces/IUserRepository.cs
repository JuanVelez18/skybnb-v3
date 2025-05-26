using domain.Entities;

namespace repository.Interfaces
{
    public interface IUserRepository : IBaseRepository<Users, Guid>
    {
        Task<Users?> GetByEmailAsync(string email);
        new Task<Users?> GetByIdAsync(Guid id);
        void AssignRole(Users user, Roles role);
    }
}
