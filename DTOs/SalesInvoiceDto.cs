namespace VehiclePartsIMS_Backend.DTOs
{
    // For creating a sales invoice (F7 & F16)
    public class SalesInvoiceCreateDto
    {
        public int CustomerId { get; set; }
        public int CreatedByStaffId { get; set; }
        public bool IsCredit { get; set; } = false;
        public List<SalesInvoiceItemDto> Items { get; set; } = new();
    }

    // Single item in sales invoice
    public class SalesInvoiceItemDto
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
    }

    // Response after creating sales invoice
    public class SalesInvoiceResponseDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly InvoiceDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public int Subtotal { get; set; }
        public int? LoyaltyDiscount { get; set; }
        public int FinalTotal { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime? PaymentDueDate { get; set; }
        public bool LoyaltyDiscountApplied { get; set; }
        public List<SalesInvoiceItemResponseDto> Items { get; set; } = new();
    }

    // Single item in response
    public class SalesInvoiceItemResponseDto
    {
        public string PartName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitSellingPrice { get; set; }
        public int TotalPrice { get; set; }
    }
}