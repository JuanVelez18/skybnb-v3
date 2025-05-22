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

        public AuthApplication(
            IUnitOfWork unitOfWork,
            IPasswordHasher<UserCredentialsDto> passwordHasher,
            IJwtGenerator jwtGenerator
        )
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
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
                birthday: userCreationDto.Birthday,
                countryId: userCreationDto.CountryId,
                phone: userCreationDto.Phone
            );
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
                throw new InvalidOperationException($"The user already exists with role {role.Name}.");
            }

            return (user, role);
        }

        public async Task<TokensDto> RegisterHost(UserCreationDto userCreationDto)
        {
            var (user, role) = await GetUserAndRole(userCreationDto, InitialData.HostRole.Id);

            _unitOfWork.Users.AssignRole(user, role);
            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: role.Id),
                RefreshToken = string.Empty
            };
        }

        public async Task<TokensDto> RegisterGuest(GuestCreationDto guestCreationDto)
        {
            var (user, role) = await GetUserAndRole(guestCreationDto, InitialData.GuestRole.Id);

            var address = new Addresses(
                street: guestCreationDto.Address.Street,
                streetNumber: guestCreationDto.Address.StreetNumber,
                intersectionNumber: guestCreationDto.Address.IntersectionNumber,
                doorNumber: guestCreationDto.Address.DoorNumber,
                cityId: guestCreationDto.Address.CityId,
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

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: role.Id),
                RefreshToken = string.Empty
            };
        }
    }
}
