using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentApplication _paymentApplication;

        public PaymentsController(IPaymentApplication paymentApplication)
        {
            _paymentApplication = paymentApplication;
        }

        [HttpPost]
        [Authorize("create:payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreationDto paymentCreationDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _paymentApplication.CreatePaymentAsync(userId, paymentCreationDto);
            return Created();
        }
    }
}