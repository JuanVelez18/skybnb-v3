using application.DTOs;

namespace presentations.Interfaces
{
    public interface IAuthPresentation
    {
        Task<TokensDto> RegisterHostAsync(UserCreationDto userCreationDto);
        Task<TokensDto> RegisterGuestAsync(GuestCreationDto guestCreationDto);
    }
}
