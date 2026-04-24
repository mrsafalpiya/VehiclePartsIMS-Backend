using VehiclePartsIMS_Backend.Data.Enums;

namespace VehiclePartsIMS_Backend.DTOs
{
    // For creating a part request (Customer)
    public class PartRequestCreateDto
    {
        public int CustomerId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    // For updating part request status (Admin/Staff)
    public class PartRequestUpdateDto
    {
        public int Id { get; set; }
        public PartRequestStatus Status { get; set; }
    }

    // Response when viewing part requests
    public class PartRequestResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}