namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class AddVehicleDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}