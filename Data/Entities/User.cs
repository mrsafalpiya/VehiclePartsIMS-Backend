using Microsoft.AspNetCore.Identity;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = String.Empty;
        new public string Email { get; set; } = String.Empty;
        public string? HomeAddress { get; set; } = null;
    }
}
