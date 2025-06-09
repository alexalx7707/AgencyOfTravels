using TravelAgency3Presentation.Models;

namespace TravelAgency3Presentation.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
        Task<Location> AddLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(int id);
        Task<IEnumerable<Location>> GetPopularLocationsAsync();
    }
}
