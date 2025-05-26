
using application.Core;
using application.DTOs;
using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using repository.Interfaces;
using System.Text.Json;

namespace application.Implementations
{
    public class ReviewsApplication : IReviewsApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsApplication(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateReview(ReviewsDto reviewsDto, Guid userId)

        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }

            var newReview = new Reviews(
                bookingId: reviewsDto.BookingId,
                propertyId: reviewsDto.PropertyId,
                guestId: reviewsDto.GuestId,
                rating: reviewsDto.Rating,
                comment: reviewsDto.Comment
        );
            await _unitOfWork.Reviews.AddAsync(newReview);


            var auditory = new Auditories(
                userId: userId,
                action: "Create Review",
                entity: "Reviews",
                entityId: newReview.Id.ToString(),
                details: JsonSerializer.Serialize(newReview, new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                }),
                timestamp: DateTime.UtcNow
            );

            await _unitOfWork.Auditories.AddAsync(auditory);

            await _unitOfWork.CommitAsync();

        }
    }
}
