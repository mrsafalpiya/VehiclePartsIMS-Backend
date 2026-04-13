using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class ServiceReview
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public required User Customer { get; set; }
        public int StarRating { get; set; }
        public required string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
