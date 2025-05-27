using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IPropertyTypesApplication _propertyTypesApplication;

        public PropertyTypesController(IPropertyTypesApplication propertyTypesApplication)
        {
            _propertyTypesApplication = propertyTypesApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _propertyTypesApplication.GetAllAsync() ?? [];
            return Ok(types);
        }
    }
}
