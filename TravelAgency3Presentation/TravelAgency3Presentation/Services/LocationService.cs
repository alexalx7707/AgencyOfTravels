using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Repositories;

namespace TravelAgency3Presentation.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locationRepository;

        public LocationService(IRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task<Location> AddLocationAsync(Location location)
        {
            location.CreatedAt = DateTime.UtcNow;
            return await _locationRepository.AddAsync(location);
        }

        public async Task UpdateLocationAsync(Location location)
        {
            await _locationRepository.UpdateAsync(location);
        }

        public async Task DeleteLocationAsync(int id)
        {
            await _locationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Location>> GetPopularLocationsAsync()
        {
            var locations = await _locationRepository.GetAllAsync();
            return locations.Where(l => l.IsPopular);
        }
    }
}
