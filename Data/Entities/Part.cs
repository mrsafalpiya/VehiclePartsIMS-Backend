using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    [Index(nameof(PartCode), IsUnique = true)]
    public class Part
    {
        public int Id { get; set; }
        public required string PartName { get; set; }
        public required string PartCode { get; set; }
        public int SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public int VendorId { get; set; }
        public required Vendor Vendor { get; set; }
    }
}
