using Microsoft.AspNetCore.Mvc;
using application.Interfaces;

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
    }
}