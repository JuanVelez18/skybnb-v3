using application.DTOs;
using application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace asp_services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsApplication _reviewsApplication;
        public ReviewsController(IReviewsApplication reviewsApplication)
        {
            _reviewsApplication = reviewsApplication;
        }
        [HttpPost]
        [Authorize("create:review")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewsDto reviewsDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _reviewsApplication.CreateReview(reviewsDto, userId);
            return Created();
        }

        [HttpDelete]
        [Authorize("delete:review")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _reviewsApplication.DeleteReview(reviewId, userId);
            return NoContent();
        }
    }
}