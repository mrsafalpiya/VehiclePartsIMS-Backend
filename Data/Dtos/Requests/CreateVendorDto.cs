namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class CreateVendorDto
    {
        public string VendorName { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
