using System.ComponentModel.DataAnnotations.Schema;
using VehiclePartsIMS_Backend.Data.Enums;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class PartRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public required User Customer { get; set; }
        public required string PartName { get; set; }
        public string? Notes { get; set; }
        public PartRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
