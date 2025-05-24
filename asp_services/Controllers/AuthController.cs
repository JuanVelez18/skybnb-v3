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
            try
            {
                var tokens = await _usersApplication.RegisterHost(userCreationDto);
                return Ok(tokens);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPost("register/guest")]
        public async Task<IActionResult> RegisterGuest([FromBody] GuestCreationDto guestCreationDto)
        {
            try
            {
                var tokens = await _usersApplication.RegisterGuest(guestCreationDto);
                return Ok(tokens);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto credentials)
        {
            try
            {
                var tokens = await _usersApplication.Login(credentials);
                return Ok(tokens);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
    }
}