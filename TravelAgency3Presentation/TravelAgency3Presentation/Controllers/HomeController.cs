using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Models.Enums;
using TravelAgency3Presentation.Services;

namespace TravelAgency.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocationService _locationService;
        private readonly IBookingService _bookingService;
        private readonly IReviewService _reviewService;
        private readonly IRecommendationService _recommendationService;

        public HomeController(
            ILogger<HomeController> logger,
            ILocationService locationService,
            IBookingService bookingService,
            IReviewService reviewService,
            IRecommendationService recommendationService)
        {
            _logger = logger;
            _locationService = locationService;
            _bookingService = bookingService;
            _reviewService = reviewService;
            _recommendationService = recommendationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new Home
                {
                    FeaturedLocations = await _locationService.GetAllLocationsAsync(),
                    RecentBookings = await _bookingService.GetAllBookingsAsync(),
                    TopRatedLocations = await _locationService.GetPopularLocationsAsync(),
                    PopularDestinations = await _recommendationService.GetPersonalizedRecommendationsAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading Home/Index page");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //public async Task<IActionResult> About()
        //{
        //    var aboutViewModel = new About
        //    {
        //        TotalLocations = await _locationService.GetLocationsCountAsync(),
        //        TotalBookings = await _bookingService.GetBookingsCountAsync(),
        //        TotalReviews = await _reviewService.GetReviewsCountAsync(),
        //        TopDestinations = await _locationService.GetTopRatedLocationsAsync(3)
        //    };

        //    return View(aboutViewModel);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> Search(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query))
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var searchResults = await _locationService.SearchLocationsAsync(query);
        //    return View(searchResults);
        //}

        [HttpPost]
        public async Task<IActionResult> Booking(Booking model)
        {
            if (!ModelState.IsValid)
            {
                // Return partial view with validation errors
                return PartialView("_QuickBookingForm", model);
            }

            try
            {
                var booking = new Booking
                {

                    LocationId = model.LocationId,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    NumberOfGuests = model.NumberOfGuests,
                    CustomerName = model.CustomerName,
                    Email = model.Email,
                    Phone = model.Phone,
                    CreatedAt = DateTime.UtcNow,
                    Status = BookingStatus.Pending

                };

                await _bookingService.AddBookingAsync(booking);

                // Return success message
                TempData["SuccessMessage"] = "Your booking request has been submitted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing quick booking");
                ModelState.AddModelError("", "There was an error processing your booking. Please try again.");
                return PartialView("_QuickBookingForm", model);
            }
        }

        public async Task<IActionResult> Testimonials()
        {
            var testimonials = await _reviewService.GetAllReviewsAsync();
            return View(testimonials);
        }

       
    }
}