using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Services;
using TravelAgency3Presentation.Models.Enums;

namespace TravelAgency3Presentation.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ILocationService _locationService;

        public BookingsController(IBookingService bookingService, ILocationService locationService)
        {
            _bookingService = bookingService;
            _locationService = locationService;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create(int? locationId)
        {
            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", locationId);
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check if location is available for the selected dates
                bool isAvailable = await _bookingService.IsLocationAvailableAsync(
                    booking.LocationId, booking.CheckInDate, booking.CheckOutDate);

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "The selected location is not available for these dates.");
                    ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", booking.LocationId);
                    return View(booking);
                }

                await _bookingService.AddBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", booking.LocationId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", booking.LocationId);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(BookingStatus)));

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bookingService.UpdateBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "Id", "Name", booking.LocationId);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(BookingStatus)));

            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/CheckAvailability
        public async Task<IActionResult> CheckAvailability(int locationId, DateTime checkIn, DateTime checkOut)
        {
            bool isAvailable = await _bookingService.IsLocationAvailableAsync(locationId, checkIn, checkOut);
            decimal totalPrice = await _bookingService.CalculateTotalPriceAsync(locationId, checkIn, checkOut, 2);

            return Json(new { isAvailable, totalPrice });
        }
    }
}
