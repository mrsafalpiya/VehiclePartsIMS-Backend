namespace VehiclePartsIMS_Backend.Data.Entities
{
    public class SalesInvoiceItem
    {
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public required SalesInvoice SalesInvoice { get; set; }
        public int PartId { get; set; }
        public required Part Part { get; set; }
        public int Quantity { get; set; }
        public int UnitSellingPrice { get; set; }
    }
}
