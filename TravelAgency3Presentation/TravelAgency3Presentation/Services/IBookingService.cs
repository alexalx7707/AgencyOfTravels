using TravelAgency3Presentation.Models;

namespace TravelAgency3Presentation.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByLocationIdAsync(int locationId);
        Task<bool> IsLocationAvailableAsync(int locationId, DateTime checkIn, DateTime checkOut);
        Task<decimal> CalculateTotalPriceAsync(int locationId, DateTime checkIn, DateTime checkOut, int guests);
       
    }
}
