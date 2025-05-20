using application.DTOs;
using application.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using repository.Configuration;
using repository.Interfaces;

namespace application.Implementations
{
    public class AuthApplication: IUsersApplication
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

        public async Task<TokensDto> RegisterHost(UserCreationDto userCreationDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(userCreationDto.Email);
            if (user != null && user.Roles.Any(r => r.Id == InitialData.HostRole.Id))
            {
                throw new InvalidOperationException("The user already exists with role Host.");
            }

            user = user ?? CreateBaseUser(userCreationDto);

            user = await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Users.AssignRole(user.Id, InitialData.HostRole.Id);

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: InitialData.HostRole.Id),
                RefreshToken = string.Empty
            };
        }

        public async Task<TokensDto> RegisterGuest(GuestCreationDto guestCreationDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(guestCreationDto.Email);
            if (user != null && user.Roles.Any(r => r.Id == InitialData.GuestRole.Id))
            {
                throw new InvalidOperationException("The user already exists with role Guest.");
            }

            user = user ?? CreateBaseUser(guestCreationDto);
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
            var guest = new Guests(
                userId: user.Id,
                addressId: address.Id
            );

            user = await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Guests.AddAsync(guest);
            await _unitOfWork.Users.AssignRole(user.Id, InitialData.GuestRole.Id);

            await _unitOfWork.CommitAsync();

            return new TokensDto
            {
                AccessToken = _jwtGenerator.GenerateAccessToken(userId: user.Id, roleId: InitialData.GuestRole.Id),
                RefreshToken = string.Empty
            };
        }
    }
}
