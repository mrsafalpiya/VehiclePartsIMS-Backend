using System.ComponentModel.DataAnnotations;

namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class UpdateCustomerProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
    }
}
