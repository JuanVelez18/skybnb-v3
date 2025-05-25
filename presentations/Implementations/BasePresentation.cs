using application.DTOs;
using Microsoft.Extensions.Options;
using presentations.Interfaces;

namespace presentations.Implementations
{
    public class BasePresentation : IBasePresentation
    {
        protected Comunication _comunication;

        public BasePresentation(Comunication comunication)
        {
            _comunication = comunication;
        }

        public void Authenticate(string accessToken, string refreshToken)
        {
            _comunication.Authenticate(accessToken, refreshToken);
        }

        public bool VerifyTokenRotation(string accessToken, string refreshToken)
        {
            return _comunication.VerifyTokenRotation(accessToken, refreshToken);
        }

        public TokensDto GetTokens()
        {
            return _comunication.GetTokens();
        }
    }
}
