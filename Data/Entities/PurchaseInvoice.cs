namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class PurchaseInvoice
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public required Vendor Vendor { get; set; }
        public DateOnly InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty; 
        
        public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
    }
}
