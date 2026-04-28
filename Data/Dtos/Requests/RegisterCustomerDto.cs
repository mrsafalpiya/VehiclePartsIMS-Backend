using System.ComponentModel.DataAnnotations;

namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class RegisterCustomerDto
    {
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
