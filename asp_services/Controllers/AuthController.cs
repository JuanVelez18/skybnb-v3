using asp_services.Core;
using asp_services.Dtos;
using domain.Core;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using application.Interfaces;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly IPasswordHasher<UserDto> _passwordHasher;
        private readonly IUsersApplication _usersApplication;

        public AuthController(IPasswordHasher<UserDto> passwordHasher, IUsersApplication usersApplication)
        {
            _jwtGenerator = new JwtGenerator(Configuration.SecretKey);
            _passwordHasher = passwordHasher;
            _usersApplication = usersApplication;
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // 1. Validar credenciales (esto es un ejemplo simplificado)
            // En una aplicación real, consultarías una base de datos, verificarías un hash de contraseña, etc.
            bool credentialsAreValid = loginModel.Username == "testuser" && loginModel.Password == "P@ssword1!";
            bool isAdmin = loginModel.Username == "adminuser" && loginModel.Password == "AdminP@ss!"; // Ejemplo

            if (credentialsAreValid || isAdmin)
            {
                string userId = isAdmin ? "admin001" : "user123";
                string userName = isAdmin ? "Admin User" : "Test User";

                // 2. Generar el token
                var token = _jwtGenerator.GenerateToken(
                    userId: userId,
                    expiresIn: TimeSpan.FromHours(1)
                );

                // 3. Devolver el token
                return Ok(new { Token = token });
            }

            return Unauthorized("Credenciales inválidas.");
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] UserDto userDto)
        {
            Users user;

            try
            {
                user = new Users(
                    dni: userDto.Dni,
                    firstName: userDto.FirstName,
                    lastName: userDto.LastName,
                    email: userDto.Email,
                    birthday: userDto.Birthday,
                    countryId: userDto.CountryId,
                    phone: userDto.Phone,
                    passwordHash: _passwordHasher.HashPassword(userDto, userDto.Password)
                );

            }
            catch (Exception error)
            {
                return BadRequest(new { error = error.Message });
            }
            
            _usersApplication.Guardar(user);

            return Ok(201);
        }
    }
}