namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class CreateAppointmentDto
    {
        public required string PreferredDate { get; set; } // "YYYY-MM-DD"
        public required string PreferredTime { get; set; } // "HH:mm"
    }
}