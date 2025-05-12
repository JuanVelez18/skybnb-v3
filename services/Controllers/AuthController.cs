using domain.Core;
using domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using repository.Implementations;
using services.Core;

namespace services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtGenerator _jwtGenerator; // Inyectado o instanciado

        public AuthController()
        {
            _jwtGenerator = new JwtGenerator(Configuration.SecretKey);
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
            bool credentialsAreValid = (loginModel.Username == "testuser" && loginModel.Password == "P@ssword1!");
            bool isAdmin = (loginModel.Username == "adminuser" && loginModel.Password == "AdminP@ss!"); // Ejemplo

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

        public class UserDto
        {
            public string dni { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set;}
            public string email { get; set;}
            public string password { get; set;}
            public DateOnly birthday { get; set;}
            public int countryId { get; set;}
            public string? phone { get; set; }
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] UserDto userDto)
        {
            var hasher = new PasswordHasher();
            var dbConexion = new DbConexion();
            Users user;

            try
            {
                user = new Users(
                    dni: userDto.dni,
                    firstName: userDto.firstName,
                    lastName: userDto.lastName,
                    email: userDto.email,
                    birthday: userDto.birthday,
                    countryId: userDto.countryId,
                    phone: userDto.phone,
                    passwordHash: 
                );
            }
            catch (Exception error)
            {
                return BadRequest(new { error = error.Message });
            }
            dbConexion.Users.Add(userDto);
            dbConexion.SaveChanges();
        }
    }
}
