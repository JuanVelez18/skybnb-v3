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
        [Authorize("create:property")]
        public async Task<IActionResult> CreateProperty([FromBody] PropertiesCreationDto propertiesCreationDto)
        {
            var hostId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _propertiesApplication.CreateProperties(propertiesCreationDto, hostId);
            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetProperties([FromQuery] PaginationOptionsDto paginationDto, [FromQuery] PropertyFiltersDto? filtersDto)
        {
            var identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? userId = identifier != null ? Guid.Parse(identifier) : null;

            var properties = await _propertiesApplication.GetPropertiesAsync(paginationDto, filtersDto, userId);
            return Ok(properties);
        }

        [HttpGet("{propertyId:guid}")]
        public async Task<IActionResult> GetPropertyDetail(Guid propertyId)
        {
            var identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? userId = identifier != null ? Guid.Parse(identifier) : null;

            var propertyDetail = await _propertiesApplication.GetDetailByIdAsync(propertyId, userId);

            return Ok(propertyDetail);
        }
    }
}
