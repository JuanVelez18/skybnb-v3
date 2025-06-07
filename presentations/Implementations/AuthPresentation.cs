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
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error: {response.Error!.Detail}");
            }
        }

        public async Task<TokensDto> RegisterGuestAsync(GuestCreationDto guestCreationDto)
        {
            var response = await _comunication.Execute<TokensDto, GuestCreationDto>("/auth/register/guest", guestCreationDto);
            if (response.Ok)
            {
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error: {response.Error!.Detail}");
            }
        }

        public async Task<TokensDto> LoginAsync(UserCredentialsDto loginDto)
        {
            var response = await _comunication.Execute<TokensDto, UserCredentialsDto>("/auth/login", loginDto);
            if (response.Ok)
            {
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error: {response.Error!.Detail}");
            }
        }
    }
}
