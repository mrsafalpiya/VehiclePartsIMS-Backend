using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    [Index(nameof(PlateNumber), IsUnique = true)]
    public class Vehicle
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public required User Customer { get; set; }
        public required string PlateNumber { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
    }
}
