using Microsoft.AspNetCore.Mvc;
using TravelAgency3Presentation.Models;

public class RecommendationsController : Controller
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationsController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    // GET: Recommendations
    public async Task<IActionResult> Index(
        string country = null,
        decimal? maxPrice = null,
        int? minRating = null,
        bool popularOnly = false)
    {
        ViewBag.Country = country;
        ViewBag.MaxPrice = maxPrice;
        ViewBag.MinRating = minRating;
        ViewBag.PopularOnly = popularOnly;

        var recommendations = await _recommendationService.GetPersonalizedRecommendationsAsync(
            country, maxPrice, minRating, popularOnly);

        return View(recommendations);
    }

    // GET: Recommendations/Similar/5
    public async Task<IActionResult> Similar(int id)
    {
        var similarLocations = await _recommendationService.GetSimilarLocationsAsync(id);
        return View(similarLocations);
    }

    // AJAX: Recommendations/GetSimilarLocations/5
    [HttpGet]
    public async Task<IActionResult> GetSimilarLocations(int id)
    {
        var similarLocations = await _recommendationService.GetSimilarLocationsAsync(id);
        return Json(similarLocations);
    }
}