namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string? HomeAddress { get; set; } = null;
        public string PasswordHash { get; set; } = String.Empty;
    }
}
