namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class VehicleResponseDto
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
