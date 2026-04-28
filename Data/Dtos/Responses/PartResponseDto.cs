namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class PartResponseDto
    {
        public int Id { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string PartCode { get; set; } = string.Empty;
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
    }
}
