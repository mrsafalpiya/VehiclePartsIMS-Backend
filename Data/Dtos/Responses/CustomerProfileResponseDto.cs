namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class CustomerProfileResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
    }
}
