using application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryApplication _countryApplication;

        public CountriesController(ICountryApplication countryApplication)
        {
            _countryApplication = countryApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryApplication.GetAllAsync() ?? [];
            return Ok(countries);
        }
    }
}
