namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
