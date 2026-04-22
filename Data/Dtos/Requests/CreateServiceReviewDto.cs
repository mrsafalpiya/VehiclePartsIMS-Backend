using System.ComponentModel.DataAnnotations;

namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class CreateServiceReviewDto
    {
        [Range(1, 5)]
        public int StarRating { get; set; }
        public required string Comment { get; set; }
    }
}