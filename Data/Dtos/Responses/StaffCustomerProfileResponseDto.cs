namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class StaffCustomerProfileResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public List<VehicleResponseDto> Vehicles { get; set; } = [];
        public List<SalesInvoiceSummaryDto> SalesHistory { get; set; } = [];
    }

    public class SalesInvoiceSummaryDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly InvoiceDate { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public int Subtotal { get; set; }
        public int? LoyaltyDiscount { get; set; }
        public int FinalTotal { get; set; }
        public List<InvoiceItemSummaryDto> Items { get; set; } = [];
    }

    public class InvoiceItemSummaryDto
    {
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitSellingPrice { get; set; }
    }
}
