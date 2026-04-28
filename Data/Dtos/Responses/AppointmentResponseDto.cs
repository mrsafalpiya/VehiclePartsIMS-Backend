namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PreferredDate { get; set; } = string.Empty;
        public string PreferredTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}