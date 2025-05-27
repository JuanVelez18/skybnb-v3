using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/property-types")]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IPropertiesApplication _propertiesApplication;

        public PropertyTypesController(IPropertiesApplication propertiesApplication)
        {
            _propertiesApplication = propertiesApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var propertyTypes = await _propertiesApplication.GetAllPropertyTypesAsync();
            return Ok(propertyTypes);
        }
    }
}
