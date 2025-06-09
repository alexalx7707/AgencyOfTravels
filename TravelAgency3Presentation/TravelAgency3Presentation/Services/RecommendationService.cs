using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Repositories;
using TravelAgency3Presentation.Services;

public class RecommendationDto
{
    public Location Location { get; set; }
    public double MatchScore { get; set; }
    public List<string> MatchReasons { get; set; } = new List<string>();
}

public interface IRecommendationService
{
    Task<List<RecommendationDto>> GetPersonalizedRecommendationsAsync(
        string preferredCountry = null,
        decimal? maxBudgetPerNight = null,
        int? rating = null,
        bool popularOnly = false);

    Task<List<RecommendationDto>> GetSimilarLocationsAsync(int locationId, int count = 3);
}

public class RecommendationService : IRecommendationService
{
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<Review> _reviewRepository;
    private readonly IReviewService _reviewService;

    public RecommendationService(
        IRepository<Location> locationRepository,
        IRepository<Review> reviewRepository,
        IReviewService reviewService)
    {
        _locationRepository = locationRepository;
        _reviewRepository = reviewRepository;
        _reviewService = reviewService;
    }

    public async Task<List<RecommendationDto>> GetPersonalizedRecommendationsAsync(
        string preferredCountry = null,
        decimal? maxBudgetPerNight = null,
        int? rating = null,
        bool popularOnly = false)
    {
        var allLocations = await _locationRepository.GetAllAsync();
        var recommendations = new List<RecommendationDto>();

        foreach (var location in allLocations)
        {
            // Skip if we're only looking for popular locations and this isn't one
            if (popularOnly && !location.IsPopular)
                continue;

            var avgRating = await _reviewService.GetAverageRatingForLocationAsync(location.Id);

            // Skip if rating filter is applied and location doesn't meet it
            if (rating.HasValue && avgRating < rating.Value)
                continue;

            // Skip if budget filter is applied and location is too expensive
            if (maxBudgetPerNight.HasValue && location.PricePerNight > maxBudgetPerNight.Value)
                continue;

            // Skip if country filter is applied and location isn't in that country
            if (!string.IsNullOrEmpty(preferredCountry) &&
                !location.Country.Equals(preferredCountry, StringComparison.OrdinalIgnoreCase))
                continue;

            // Calculate match score (0-100)
            double matchScore = 70; // Base score
            var matchReasons = new List<string>();

            // Adjust score based on popularity
            if (location.IsPopular)
            {
                matchScore += 10;
                matchReasons.Add("Popular destination");
            }

            // Adjust score based on ratings
            if (avgRating >= 4.5)
            {
                matchScore += 15;
                matchReasons.Add("Highly rated by travelers");
            }
            else if (avgRating >= 4.0)
            {
                matchScore += 10;
                matchReasons.Add("Well rated by travelers");
            }

            // Adjust score based on price (lower price = higher score)
            if (location.PricePerNight < 100)
            {
                matchScore += 15;
                matchReasons.Add("Budget-friendly option");
            }
            else if (location.PricePerNight < 200)
            {
                matchScore += 5;
                matchReasons.Add("Moderately priced");
            }

            // Cap score at 100
            matchScore = Math.Min(matchScore, 100);

            recommendations.Add(new RecommendationDto
            {
                Location = location,
                MatchScore = matchScore,
                MatchReasons = matchReasons
            });
        }

        // Sort by match score (descending)
        return recommendations
            .OrderByDescending(r => r.MatchScore)
            .ToList();
    }

    public async Task<List<RecommendationDto>> GetSimilarLocationsAsync(int locationId, int count = 3)
    {
        var allLocations = await _locationRepository.GetAllAsync();
        var targetLocation = await _locationRepository.GetByIdAsync(locationId);

        if (targetLocation == null)
            return new List<RecommendationDto>();

        var similarities = new List<RecommendationDto>();

        foreach (var location in allLocations)
        {
            if (location.Id == locationId)
                continue; // Skip the target location

            var matchReasons = new List<string>();
            double similarityScore = 0;

            // Same country bonus
            if (location.Country.Equals(targetLocation.Country, StringComparison.OrdinalIgnoreCase))
            {
                similarityScore += 30;
                matchReasons.Add($"Also located in {location.Country}");
            }

            // Price similarity (within 30% of target price)
            var priceDifference = Math.Abs(location.PricePerNight - targetLocation.PricePerNight) / targetLocation.PricePerNight;
            if (priceDifference <= 0.3m)
            {
                similarityScore += 30 * (1 - (double)priceDifference);

                if (location.PricePerNight < targetLocation.PricePerNight)
                    matchReasons.Add("Lower price point");
                else if (location.PricePerNight > targetLocation.PricePerNight)
                    matchReasons.Add("Premium option");
                else
                    matchReasons.Add("Similar price point");
            }

            // Popularity similarity
            if (location.IsPopular == targetLocation.IsPopular)
            {
                similarityScore += 20;
                if (location.IsPopular)
                    matchReasons.Add("Equally popular destination");
            }

            // Check review ratings similarity
            var targetRating = await _reviewService.GetAverageRatingForLocationAsync(targetLocation.Id);
            var locationRating = await _reviewService.GetAverageRatingForLocationAsync(location.Id);

            if (Math.Abs(locationRating - targetRating) <= 0.5)
            {
                similarityScore += 20;
                matchReasons.Add("Similarly rated by travelers");
            }
            else if (locationRating > targetRating + 0.5)
            {
                similarityScore += 10;
                matchReasons.Add("Higher rated by travelers");
            }

            similarities.Add(new RecommendationDto
            {
                Location = location,
                MatchScore = similarityScore,
                MatchReasons = matchReasons
            });
        }

        // Return the top similar locations
        return similarities
            .OrderByDescending(s => s.MatchScore)
            .Take(count)
            .ToList();
    }
}