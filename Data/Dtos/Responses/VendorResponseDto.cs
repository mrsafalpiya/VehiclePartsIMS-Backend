namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class VendorResponseDto
    {
        public int Id { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
