using System.Security.Claims;
using application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public UsersController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        [HttpGet("me")]
        [Authorize("read:user")]
        public async Task<IActionResult> GetUserSummaryByIdAsync()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userSummary = await _userApplication.GetUserSummaryByIdAsync(userId);

            return Ok(userSummary);
        }
    }
}
