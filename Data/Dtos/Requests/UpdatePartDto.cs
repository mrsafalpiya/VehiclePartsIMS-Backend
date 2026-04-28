namespace VehiclePartsIMS_Backend.Data.Dtos.Requests
{
    public class UpdatePartDto
    {
        public string PartName { get; set; } = string.Empty;
        public string PartCode { get; set; } = string.Empty;
        public int SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public int VendorId { get; set; }
    }
}
