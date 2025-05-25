using application.DTOs;
using Microsoft.Extensions.Options;
using presentations.Interfaces;

namespace presentations.Implementations
{
    public class BasePresentation : IBasePresentation
    {
        protected Comunication _comunication;

        public BasePresentation(IOptions<PresentationConfiguration> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options cannot be null.");

            if (string.IsNullOrEmpty(options.Value.Host))
                throw new ArgumentNullException(nameof(options.Value.Host), "Host cannot be null or empty.");

            _comunication = new Comunication(options.Value.Host);
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
