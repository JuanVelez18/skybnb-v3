using application.DTOs;

namespace application.Interfaces
{
    public interface IReviewsApplication
    {
        Task CreateReview(ReviewsDto reviewsDto, Guid userId);
    }
}
