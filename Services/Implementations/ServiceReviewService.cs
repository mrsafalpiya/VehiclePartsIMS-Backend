using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class ServiceReviewService : IServiceReviewService
    {
        private readonly AppDbContext _context;

        public ServiceReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceReviewResponseDto> SubmitReviewAsync(int customerId, CreateServiceReviewDto dto)
        {
            var customer = await _context.Users.FindAsync(customerId)
                           ?? throw new KeyNotFoundException("Customer not found.");

            var review = new ServiceReview
            {
                CustomerId = customerId,
                Customer = customer,
                StarRating = dto.StarRating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.ServiceReviews.Add(review);
            await _context.SaveChangesAsync();

            return new ServiceReviewResponseDto
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                CustomerName = customer.FullName,
                StarRating = review.StarRating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}