namespace application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateAccessToken(Guid userId, int? roleId);
        string GenerateRefreshToken();
    }
}
