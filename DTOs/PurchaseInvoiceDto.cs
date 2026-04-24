namespace VehiclePartsIMS_Backend.DTOs
{
    // For creating a purchase invoice (F4)
    public class PurchaseInvoiceCreateDto
    {
        public int VendorId { get; set; }
        public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
    }

    // Single item in purchase invoice
    public class PurchaseInvoiceItemDto
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
        public int UnitCostPrice { get; set; }  // In cents
    }

    // Response after creating purchase invoice
    public class PurchaseInvoiceResponseDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly InvoiceDate { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public int TotalAmount { get; set; }
        public List<PurchaseInvoiceItemResponseDto> Items { get; set; } = new();
    }

    // Single item in response
    public class PurchaseInvoiceItemResponseDto
    {
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitCostPrice { get; set; }
        public int TotalPrice { get; set; }
    }
}