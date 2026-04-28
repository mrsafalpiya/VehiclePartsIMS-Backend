namespace VehiclePartsIMS_Backend.Data.Dtos.Responses
{
    public class PurchaseHistoryItemDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public List<SalesInvoiceLineItemDto> LineItems { get; set; } = [];
        public int Subtotal { get; set; }
        public int? LoyaltyDiscount { get; set; }
        public int FinalTotal { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }

    public class SalesInvoiceLineItemDto
    {
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitSellingPrice { get; set; }
        public int LineTotal { get; set; }
    }
}