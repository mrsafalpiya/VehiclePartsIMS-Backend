namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class CreatePartRequestDto
    {
        public required string PartName { get; set; }
        public string? Notes { get; set; }
    }
}