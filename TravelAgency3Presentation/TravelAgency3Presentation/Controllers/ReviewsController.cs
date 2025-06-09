using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Services;

namespace TravelAgency3Presentation.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ILocationService _locationService;

        public ReviewsController(IReviewService reviewService, ILocationService locationService)
        {
            _reviewService = reviewService;
            _locationService = locationService;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return View(reviews);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create(int? locationId)
        {
            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", locationId);
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.AddReviewAsync(review);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", review.LocationId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", review.LocationId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _reviewService.UpdateReviewAsync(review);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", review.LocationId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Reviews/LocationReviews/5
        public async Task<IActionResult> LocationReviews(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            var reviews = await _reviewService.GetReviewsByLocationIdAsync(id);
            ViewBag.Location = location;
            ViewBag.AverageRating = await _reviewService.GetAverageRatingForLocationAsync(id);

            return View(reviews);
        }
    }
}
