using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly AppDbContext _context;

        public HistoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseHistoryItemDto>> GetPurchaseHistoryAsync(int customerId)
        {
            var invoices = await _context.SalesInvoices
                .Include(i => i.Customer)
                .Include(i => i.SalesInvoiceItems)
                    .ThenInclude(item => item.Part)
                .Where(i => i.CustomerId == customerId)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            return invoices.Select(i => new PurchaseHistoryItemDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate.ToString("yyyy-MM-dd"),
                LineItems = i.SalesInvoiceItems.Select(item => new SalesInvoiceLineItemDto
                {
                    PartName = item.Part.PartName,
                    Quantity = item.Quantity,
                    UnitSellingPrice = item.UnitSellingPrice,
                    LineTotal = item.Quantity * item.UnitSellingPrice
                }).ToList(),
                Subtotal = i.Subtotal,
                LoyaltyDiscount = i.LoyaltyDiscount,
                FinalTotal = i.FinalTotal,
                PaymentStatus = i.PaymentStatus.ToString()
            }).ToList();
        }

        public async Task<List<AppointmentResponseDto>> GetAppointmentHistoryAsync(int customerId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return appointments.Select(a => new AppointmentResponseDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer.FullName,
                PreferredDate = a.PreferredDate.ToString("yyyy-MM-dd"),
                PreferredTime = a.PreferredTime.ToString("HH:mm"),
                Status = a.Status.ToString(),
                CreatedAt = a.CreatedAt
            }).ToList();
        }
    }
}