using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using VehiclePartsIMS_Backend.Data.Enums;

namespace VehiclePartsIMS_Backend.Data.Entities
{
    [Index(nameof(InvoiceNumber), IsUnique = true)]
    public class SalesInvoice
    {
        public int Id { get; set; }
        public required string InvoiceNumber { get; set; }
        public DateOnly InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public required User Customer { get; set; }
        public int CreatedByStaffId { get; set; }
        [ForeignKey(nameof(CreatedByStaffId))]
        public required User CreatedByStaff { get; set; }
        public SalesInvoicePaymentStatus PaymentStatus { get; set; } = SalesInvoicePaymentStatus.Unpaid;
        public int Subtotal { get; set; }
        public int? LoyaltyDiscount { get; set; }
        public int FinalTotal { get; set; }
    }
}
