using System.ComponentModel.DataAnnotations.Schema;
using TravelAgency3Presentation.Models.Enums;

namespace TravelAgency3Presentation.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(LocationId))]
        public virtual Location? Location { get; set; }
    }
}
