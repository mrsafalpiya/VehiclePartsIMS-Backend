using System.ComponentModel.DataAnnotations.Schema;
using VehiclePartsIMS_Backend.Data.Enums;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public required User Customer { get; set; }
        public DateOnly PreferredDate { get; set; }
        public TimeOnly PreferredTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }
}
