
using application.Core;
using application.DTOs;
using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using repository.Configuration;
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

        public async Task CreateReview(ReviewsDto reviewsDto, Guid guestId)

        {
            var user = await _unitOfWork.Users.GetByIdAsync(guestId);
            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }

            var property = await _unitOfWork.Properties.GetByIdAsync(reviewsDto.PropertyId);
            if (property == null) 
            { 
                throw new NotFoundApplicationException("Property not found.");
            }

            var booking = await _unitOfWork.Bookings.GetByIdAsync(reviewsDto.BookingId);

            if (booking == null)
            {
                throw new NotFoundApplicationException("Booking not found.");
            }

            var newReview = new Reviews(
                reviewsDto.BookingId,
                reviewsDto.PropertyId,
                guestId,
                reviewsDto.Rating,
                reviewsDto.Comment
        );
            newReview.Booking = booking;
            newReview.Property = property;
            newReview.Guest = user;


            await _unitOfWork.Reviews.AddAsync(newReview);


            var auditory = new Auditories(
                userId: guestId,
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

        public async Task DeleteReview(Guid reviewId, Guid guestId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review == null)
            {
                throw new NotFoundApplicationException("Review not found.");
            }
            var user = await _unitOfWork.Users.GetByIdAsync(guestId);
            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }
            if (review.GuestId != guestId)
            {
                throw new UnauthorizedApplicationException("You are not authorized to delete this review.");
            }

            var auditory = new Auditories(
                userId: guestId,
                action: "Delete Review",
                entity: "Reviews",
                entityId: review.Id.ToString(),
                details: JsonSerializer.Serialize(review, new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                }),
                timestamp: DateTime.UtcNow
            );

            await _unitOfWork.Auditories.AddAsync(auditory);
            _unitOfWork.Reviews.Delete(review);          
            await _unitOfWork.CommitAsync();
        }
        
    }
}
