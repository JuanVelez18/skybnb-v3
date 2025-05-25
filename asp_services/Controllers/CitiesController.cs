using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICitytApplication _cityApplication;
        public CitiesController(ICitytApplication cityApplication)
        {
            _cityApplication = cityApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _cityApplication.GetAllAsync() ?? [];
            return Ok(cities);
        }
    }
}
