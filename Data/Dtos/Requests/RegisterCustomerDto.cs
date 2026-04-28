namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class RegisterCustomerDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<AddVehicleDto> Vehicles { get; set; } = [];
    }
}