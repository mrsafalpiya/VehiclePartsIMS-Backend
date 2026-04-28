using System.ComponentModel.DataAnnotations;

namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class UpdateStaffDto
    {
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Null means no password change
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
