using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesApplication _propertiesApplication;

        public PropertiesController(IPropertiesApplication propertiesApplication)
        {
            _propertiesApplication = propertiesApplication;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProperty([FromBody] PropertiesCreationDto propertiesCreationDto)
        {
            var hostId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _propertiesApplication.CreateProperties(propertiesCreationDto, hostId);
            return Created();
    }
        
    }
}
