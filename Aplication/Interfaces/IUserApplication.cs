using application.DTOs;

namespace application.Interfaces
{
    public interface IUserApplication
    {
        Task<UserSummaryDto> GetUserSummaryByIdAsync(Guid id);
    }
}
