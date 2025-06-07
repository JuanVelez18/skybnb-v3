using application.Core;
using application.DTOs;
using application.Interfaces;
using repository.Interfaces;

namespace application.Implementations
{
    public class UserApplication : IUserApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserApplication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserSummaryDto> GetUserSummaryByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Customers.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundApplicationException($"User not found.");
            }

            var permissions = await _unitOfWork.Customers.GetUserPermissionsAsync(id);

            return new UserSummaryDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.Roles?.Select(role => role.Name).ToList() ?? new List<string>(),
                Permissions = permissions.Select(permission => permission.Name).ToList()
            };
        }
    }
}
