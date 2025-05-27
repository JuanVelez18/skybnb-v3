using application.DTOs;
using application.Implementations;
using application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsApplication _bookingsApplication;
        public BookingsController(IBookingsApplication bookingsApplication)
        {
            _bookingsApplication = bookingsApplication;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] BookingsDto bookingsDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _bookingsApplication.CreateBooking(bookingsDto, userId);
            return Created();
        }
    }
}
