using System.Text.Json;
using application.Core;
using application.DTOs;
using application.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using repository.Configuration;
using repository.Interfaces;

namespace application.Implementations
{
    public class AuthApplication : IUsersApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<UserCredentialsDto> _passwordHasher;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ITokenHasher _tokenHasher;

        public AuthApplication(
            IUnitOfWork unitOfWork,
            IPasswordHasher<UserCredentialsDto> passwordHasher,
            IJwtGenerator jwtGenerator,
            ITokenHasher tokenHasher
        )
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
            _tokenHasher = tokenHasher;
        }

        private Users CreateBaseUser(UserCreationDto userCreationDto)
        {
            return new Users(
                dni: userCreationDto.Dni,
                firstName: userCreationDto.FirstName,
                lastName: userCreationDto.LastName,
                email: userCreationDto.Email,
                passwordHash: _passwordHasher.HashPassword(
                    userCreationDto.ToCredentials(),
                    userCreationDto.Password
                ),
                birthday: userCreationDto.Birthday!.Value,
                countryId: userCreationDto.CountryId!.Value,
                phone: userCreationDto.Phone
            );
        }

        private (RefreshTokens, string) CreateRefreshToken(Users user)
        {
            var token = _jwtGenerator.GenerateRefreshToken();
            var hashedToken = _tokenHasher.HashToken(token);
            var now = DateTime.UtcNow;
            var expiresAt = now.AddDays(7);

            var entity = new RefreshTokens()
            {
                TokenValue = hashedToken,
                UserId = user.Id,
                IssuedAt = now,
                ExpiresAt = expiresAt,
                Used = false
            };

            return (entity, token);
        }

        private async Task<(Users, Roles)> GetUserAndRole(UserCreationDto userDto, int roleId)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            if (role == null)
            {
                throw new InvalidOperationException($"The role {roleId} does not exist.");
            }

            var user = await _unitOfWork.Users.GetByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = CreateBaseUser(userDto);
                user = await _unitOfWork.Users.AddAsync(user);
            }
            else if (user.Roles.Any(r => r.Id == roleId))
            {
                throw new InvalidDataApplicationException($"The user already exists with role {role.Name}.");
            }

            return (user, role);
        }

        public async Task<TokensDto> RegisterHost(UserCreationDto userCreationDto)
        {
            var (user, role) = await GetUserAndRole(userCreationDto, InitialData.HostRole.Id);
            _unitOfWork.Users.AssignRole(user, role);

            var auditory = new Auditories(
                userId: user.Id,
                action: "Register host",
                entity: "User",
                entityId: user.Id.ToString(),
                details: JsonSerializer.Serialize(user),
                timestamp: DateTime.UtcNow
            );
            await _unitOfWork.Auditories.AddAsync(auditory);

            var (refreshTokenEntity, refreshToken) = CreateRefreshToken(user);
            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: role.Id),
                RefreshToken = refreshToken
            };
        }

        public async Task<TokensDto> RegisterGuest(GuestCreationDto guestCreationDto)
        {
            var (user, role) = await GetUserAndRole(guestCreationDto, InitialData.GuestRole.Id);

            var address = new Addresses(
                street: guestCreationDto.Address.Street,
                streetNumber: guestCreationDto.Address.StreetNumber!.Value,
                intersectionNumber: guestCreationDto.Address.IntersectionNumber!.Value,
                doorNumber: guestCreationDto.Address.DoorNumber!.Value,
                cityId: guestCreationDto.Address.CityId!.Value,
                complement: guestCreationDto.Address.Complement,
                latitude: guestCreationDto.Address.Latitude,
                longitude: guestCreationDto.Address.Longitude
            );
            address = await _unitOfWork.Addresses.AddAsync(address);
            _unitOfWork.Users.AssignRole(user, role);

            var guest = new Guests(
                userId: user.Id,
                addressId: address.Id
            );
            await _unitOfWork.Guests.AddAsync(guest);

            var auditoryNow = DateTime.UtcNow;
            var userAuditory = new Auditories(
                userId: user.Id,
                action: "Register guest",
                entity: "User",
                entityId: user.Id.ToString(),
                details: JsonSerializer.Serialize(user, new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                }),
                timestamp: auditoryNow
            );
            var addressAuditory = new Auditories(
                userId: user.Id,
                action: "Register guest address",
                entity: "Address",
                entityId: address.Id.ToString(),
                details: JsonSerializer.Serialize(address),
                timestamp: auditoryNow
            );
            await _unitOfWork.Auditories.AddAsync(userAuditory);
            await _unitOfWork.Auditories.AddAsync(addressAuditory);

            var (refreshTokenEntity, refreshToken) = CreateRefreshToken(user);
            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: role.Id),
                RefreshToken = refreshToken
            };
        }

        public async Task<TokensDto> Login(UserCredentialsDto credentials)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(credentials.Email);
            if (user == null)
            {
                throw new InvalidDataApplicationException("Invalid email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(
                credentials,
                user.PasswordHash,
                credentials.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidDataApplicationException("Invalid email or password.");
            }

            var (refreshTokenEntity, refreshToken) = CreateRefreshToken(user);
            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);

            var auditory = new Auditories(
                userId: user.Id,
                action: "User login",
                timestamp: DateTime.UtcNow,
                entity: null,
                entityId: null,
                details: null
            );
            await _unitOfWork.Auditories.AddAsync(auditory);

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: null),
                RefreshToken = refreshToken
            };
        }

        public async Task<TokensDto> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var hashedToken = _tokenHasher.HashToken(refreshTokenDto.RefreshToken);
            var refreshToken = await _unitOfWork.RefreshTokens.GetByHashedTokenAsync(hashedToken);
            if (refreshToken == null || !refreshToken.IsActive)
            {
                throw new UnauthorizedApplicationException("Expired or invalid refresh token.");
            }

            var (newRefreshTokenEntity, newRefreshToken) = CreateRefreshToken(refreshToken.User!);
            await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity);
            refreshToken.ReplaceBy(newRefreshTokenEntity);

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(
                    userId: refreshToken.User!.Id,
                    roleId: null
                ),
                RefreshToken = newRefreshToken
            };
        }

        public async Task Logout(Guid userId, string refreshToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }

            var hashedToken = _tokenHasher.HashToken(refreshToken);
            var token = await _unitOfWork.RefreshTokens.GetByHashedTokenAsync(hashedToken);
            if (token == null || !token.IsActive) return;

            if (token.UserId != user.Id)
            {
                throw new UnauthorizedApplicationException("This token does not belong to the user.");
            }

            token.Revoke();

            var auditory = new Auditories(
                userId: user.Id,
                action: "User logout",
                timestamp: DateTime.UtcNow,
                entity: null,
                entityId: null,
                details: null
            );
            await _unitOfWork.Auditories.AddAsync(auditory);

            await _unitOfWork.CommitAsync();
        }
    }
}
