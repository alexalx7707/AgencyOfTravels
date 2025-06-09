using TravelAgency3Presentation.Models;

namespace TravelAgency3Presentation.Models
{
    public class Home
    {
        public IEnumerable<Location>? FeaturedLocations { get; set; }
        public IEnumerable<Booking>? RecentBookings { get; set; }
        public IEnumerable<Location>? TopRatedLocations { get; set; }
        public IEnumerable<RecommendationDto>? PopularDestinations { get; set; }
    }
}