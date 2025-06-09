using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Models.Enums;
using TravelAgency3Presentation.Repositories;

namespace TravelAgency3Presentation.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Location> _locationRepository;

        public BookingService(IRepository<Booking> bookingRepository, IRepository<Location> locationRepository)
        {
            _bookingRepository = bookingRepository;
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            booking.CreatedAt = DateTime.UtcNow;
            booking.Status = BookingStatus.Pending;

            // Calculate total price
            var location = await _locationRepository.GetByIdAsync(booking.LocationId);
            int numberOfNights = (int)(booking.CheckOutDate - booking.CheckInDate).TotalDays;
            booking.TotalPrice = location.PricePerNight * numberOfNights;

            return await _bookingRepository.AddAsync(booking);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByLocationIdAsync(int locationId)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Where(b => b.LocationId == locationId);
        }

        public async Task<bool> IsLocationAvailableAsync(int locationId, DateTime checkIn, DateTime checkOut)
        {
            var bookings = await GetBookingsByLocationIdAsync(locationId);
            return !bookings.Any(b =>
                b.Status != BookingStatus.Cancelled &&
                ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||
                 (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
                 (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate))
            );
        }

        public async Task<decimal> CalculateTotalPriceAsync(int locationId, DateTime checkIn, DateTime checkOut, int guests)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);
            int numberOfNights = (int)(checkOut - checkIn).TotalDays;
            return location.PricePerNight * numberOfNights;
        }
    }
}
