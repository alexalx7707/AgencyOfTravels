using TravelAgency3Presentation.Models;

namespace TravelAgency3Presentation.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task<Review> AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByLocationIdAsync(int locationId);
        Task<double> GetAverageRatingForLocationAsync(int locationId);
    }
}
