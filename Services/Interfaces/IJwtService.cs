namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(int userId, string name, string email, string role);
    }
}
