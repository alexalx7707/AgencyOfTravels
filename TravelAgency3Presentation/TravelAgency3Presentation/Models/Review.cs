using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency3Presentation.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(LocationId))]
        public virtual Location? Location { get; set; }
    }
}
