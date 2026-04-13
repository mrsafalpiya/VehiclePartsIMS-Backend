namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class PurchaseInvoiceItem
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public required PurchaseInvoice PurchaseInvoice { get; set; }
        public int PartId { get; set; }
        public required Part Part { get; set; }
        public int Quantity { get; set; }
        public int UnitCostPrice { get; set; }
    }
}
