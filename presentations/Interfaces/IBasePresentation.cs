using application.DTOs;

namespace presentations.Interfaces
{
    public interface IBasePresentation
    {
        void Authenticate(string accessToken, string refreshToken);
        bool VerifyTokenRotation(string accessToken, string refreshToken);
        TokensDto GetTokens();
    }
}
