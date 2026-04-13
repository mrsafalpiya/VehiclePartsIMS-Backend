namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class Vendor
    {
        public int Id { get; set; }
        public required string VendorName { get; set; }
        public required string ContactPersonName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }
}
