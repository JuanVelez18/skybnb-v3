using domain.Entities;

namespace repository.Interfaces
{
    public interface IUserRepository: IBaseRepository<Users, Guid>
    {
        Task<Users?> GetByEmailAsync(string email);
        Task AssignRole(Users user, int roleId);
    }
}
