using Microsoft.AspNetCore.Mvc;
using application.Interfaces;
using application.DTOs;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersApplication _usersApplication;

        public AuthController(IUsersApplication usersApplication)
        {
            _usersApplication = usersApplication;
        }

        [HttpPost("register/host")]
        public async Task<IActionResult> RegisterHost([FromBody] UserCreationDto userCreationDto)
        {
            var tokens = await _usersApplication.RegisterHost(userCreationDto);
            return Ok(tokens);
        }

        [HttpPost("register/guest")]
        public async Task<IActionResult> RegisterGuest([FromBody] GuestCreationDto guestCreationDto)
        {
            var tokens = await _usersApplication.RegisterGuest(guestCreationDto);
            return Ok(tokens);
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto credentials)
        {
            var tokens = await _usersApplication.Login(credentials);
            return Ok(tokens);
        }
    }
}