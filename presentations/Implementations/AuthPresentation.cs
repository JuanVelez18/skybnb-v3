using application.DTOs;
using Microsoft.Extensions.Options;
using presentations.Interfaces;

namespace presentations.Implementations
{
    public class AuthPresentation : IAuthPresentation
    {
        private readonly PresentationConfiguration _configuration;
        private readonly Comunication _comunication;

        public AuthPresentation(IOptions<PresentationConfiguration> options)
        {
            _configuration = options.Value;
            if (string.IsNullOrEmpty(_configuration.Host))
                throw new ArgumentNullException(nameof(_configuration.Host), "Host cannot be null or empty.");

            _comunication = new Comunication(_configuration.Host);
        }

        public async Task<TokensDto> RegisterHostAsync(UserCreationDto userCreationDto)
        {
            var response = await _comunication.Execute<TokensDto, UserCreationDto>("/auth/register/host", userCreationDto);
            if (response.Ok)
            {
                _comunication.Authenticate(response.Data!.AccessToken, response.Data.RefreshToken);
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error: {response.Message}");
            }
        }

        public async Task<TokensDto> RegisterGuestAsync(GuestCreationDto guestCreationDto)
        {
            var response = await _comunication.Execute<TokensDto, GuestCreationDto>("/auth/register/guest", guestCreationDto);
            if (response.Ok)
            {
                _comunication.Authenticate(response.Data!.AccessToken, response.Data.RefreshToken);
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error: {response.Message}");
            }
        }
    }
}
