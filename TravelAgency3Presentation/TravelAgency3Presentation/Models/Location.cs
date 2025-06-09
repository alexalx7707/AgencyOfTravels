using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace TravelAgency3Presentation.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPopular { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
