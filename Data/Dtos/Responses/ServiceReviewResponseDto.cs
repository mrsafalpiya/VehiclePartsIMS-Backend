namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class ServiceReviewResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int StarRating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}