using domain.Entities;

namespace repository.Interfaces
{
    public interface IRefreshTokenRepository: IBaseRepository<RefreshTokens, long>
    {
        Task<RefreshTokens?> GetByHashedTokenAsync(string hashedToken);
    }
}
