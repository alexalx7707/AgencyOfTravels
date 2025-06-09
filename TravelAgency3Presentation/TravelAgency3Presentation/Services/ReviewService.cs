using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Repositories;

namespace TravelAgency3Presentation.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepository;

        public ReviewService(IRepository<Review> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _reviewRepository.GetByIdAsync(id);
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            return await _reviewRepository.AddAsync(review);
        }

        public async Task UpdateReviewAsync(Review review)
        {
            await _reviewRepository.UpdateAsync(review);
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _reviewRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByLocationIdAsync(int locationId)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Where(r => r.LocationId == locationId);
        }

        public async Task<double> GetAverageRatingForLocationAsync(int locationId)
        {
            var reviews = await GetReviewsByLocationIdAsync(locationId);
            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }
}
