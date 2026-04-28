namespace VehiclePartsIMS_Backend.DTOs
{
    // For loyalty discount calculation (F16)
    public class LoyaltyDiscountResult
    {
        public bool IsApplicable { get; set; }
        public int DiscountPercentage { get; set; }
        public int DiscountAmount { get; set; }
        public int OriginalTotal { get; set; }
        public int DiscountedTotal { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}