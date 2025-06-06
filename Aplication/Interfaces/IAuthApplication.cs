﻿using application.DTOs;
using domain.Entities;

namespace application.Interfaces
{
    public interface IUsersApplication
    {
        Task<TokensDto> RegisterHost(UserCreationDto userCreationDto);
        Task<TokensDto> RegisterGuest(GuestCreationDto userCreationDto);
        Task<TokensDto> Login(UserCredentialsDto credentials);
        Task<TokensDto> RefreshToken(RefreshTokenDto refreshTokenDto);
        Task Logout(Guid userId, string refreshToken);
    }
}