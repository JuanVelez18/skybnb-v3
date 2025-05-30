using domain.Entities;

namespace repository.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<Customers, Guid>
    {
        Task<Customers?> GetByEmailAsync(string email);
        new Task<Customers?> GetByIdAsync(Guid id);

        Task<List<Permissions>> GetUserPermissionsAsync(Guid userId);
        void AssignRole(Customers user, Roles role);
    }
}
