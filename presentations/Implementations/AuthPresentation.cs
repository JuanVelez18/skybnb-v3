using application.DTOs;
using Microsoft.Extensions.Options;
using presentations.Interfaces;

namespace presentations.Implementations
{
    public class AuthPresentation : BasePresentation, IAuthPresentation
    {
        public AuthPresentation(Comunication comunication) : base(comunication)
        {
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
