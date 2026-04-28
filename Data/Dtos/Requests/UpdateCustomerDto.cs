namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class UpdateCustomerDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
    }
}