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
        [Authorize("create:booking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingsDto bookingsDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _bookingsApplication.CreateBooking(bookingsDto, userId);
            return Created();
        }

        [HttpGet]
        [Authorize("read:booking")]
        public async Task<IActionResult> GetBookingsByUserId(
            [FromQuery] PaginationOptionsDto paginationDto,
            [FromQuery] BookingFiltersDto? filtersDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookingsPage = await _bookingsApplication.GetBookingsByUserIdAsync(userId, paginationDto, filtersDto);
            return Ok(bookingsPage);
        }
    }
}
