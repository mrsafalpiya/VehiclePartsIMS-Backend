using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Data.Enums;
using VehiclePartsIMS_Backend.DTOs;

namespace VehiclePartsIMS_Backend.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InvoiceService> _logger;
        
        // F16: Loyalty program constants
        private const int LoyaltyThresholdCents = 5000;  
        private const int LoyaltyDiscountPercent = 10;

        public InvoiceService(AppDbContext context, ILogger<InvoiceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // F4: Create purchase invoice 
        public async Task<PurchaseInvoiceResponseDto> CreatePurchaseInvoiceAsync(PurchaseInvoiceCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var vendor = await _context.Vendors.FindAsync(dto.VendorId);
                if (vendor == null)
                    throw new KeyNotFoundException($"Vendor with ID {dto.VendorId} not found");

                string invoiceNumber = GeneratePurchaseInvoiceNumber();
                int totalAmount = 0;
                var items = new List<PurchaseInvoiceItem>();

                foreach (var item in dto.Items)
                {
                    var part = await _context.Parts.FindAsync(item.PartId);
                    if (part == null)
                        throw new KeyNotFoundException($"Part with ID {item.PartId} not found");

                    int itemTotal = item.Quantity * item.UnitCostPrice;
                    totalAmount += itemTotal;

                    items.Add(new PurchaseInvoiceItem
                    {
                        PurchaseInvoice = null!,
                        Part = part,
                        Quantity = item.Quantity,
                        UnitCostPrice = item.UnitCostPrice
                    });

                    part.StockQuantity += item.Quantity;
                    _context.Parts.Update(part);
                }

                var invoice = new PurchaseInvoice
                {
                    VendorId = dto.VendorId,
                    Vendor = vendor,
                    InvoiceDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    InvoiceNumber = invoiceNumber,
                    Items = items
                };

                _context.PurchaseInvoices.Add(invoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Purchase invoice {InvoiceNumber} created for vendor {VendorName}", 
                    invoiceNumber, vendor.VendorName);

                return new PurchaseInvoiceResponseDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoiceDate = invoice.InvoiceDate,
                    VendorName = vendor.VendorName,
                    TotalAmount = totalAmount,
                    Items = items.Select(i => new PurchaseInvoiceItemResponseDto
                    {
                        PartName = i.Part.PartName,
                        Quantity = i.Quantity,
                        UnitCostPrice = i.UnitCostPrice,
                        TotalPrice = i.Quantity * i.UnitCostPrice
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating purchase invoice");
                throw;
            }
        }

        // F7 & F16: Create sales invoice with loyalty 
        public async Task<SalesInvoiceResponseDto> CreateSalesInvoiceAsync(SalesInvoiceCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var customer = await _context.Users.FindAsync(dto.CustomerId);
                if (customer == null)
                    throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found");

                var staff = await _context.Users.FindAsync(dto.CreatedByStaffId);
                if (staff == null)
                    throw new KeyNotFoundException($"Staff with ID {dto.CreatedByStaffId} not found");

                string invoiceNumber = GenerateSalesInvoiceNumber();
                int subtotal = 0;
                var items = new List<SalesInvoiceItem>();

                foreach (var item in dto.Items)
                {
                    var part = await _context.Parts.FindAsync(item.PartId);
                    if (part == null)
                        throw new KeyNotFoundException($"Part with ID {item.PartId} not found");

                    if (part.StockQuantity < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for {part.PartName}. Available: {part.StockQuantity}");

                    int itemTotal = item.Quantity * part.SellingPrice;
                    subtotal += itemTotal;

                    items.Add(new SalesInvoiceItem
                    {
                        SalesInvoice = null!,
                        Part = part,
                        Quantity = item.Quantity,
                        UnitSellingPrice = part.SellingPrice
                    });

                    part.StockQuantity -= item.Quantity;
                    _context.Parts.Update(part);
                }

                // F16: LOYALTY DISCOUNT CALCULATION (10% off for purchases OVER 5000 cents = $50.00)
                var loyaltyResult = new LoyaltyDiscountResult
                {
                    OriginalTotal = subtotal,
                    DiscountPercentage = 0,
                    DiscountAmount = 0,
                    DiscountedTotal = subtotal,
                    IsApplicable = false,
                    Message = string.Empty
                };

                int? loyaltyDiscount = null;
                
                // Use > instead of >= (must be MORE THAN 5000, not equal to)
                if (subtotal > LoyaltyThresholdCents)
                {
                    loyaltyDiscount = (subtotal * LoyaltyDiscountPercent) / 100;
                    loyaltyResult.IsApplicable = true;
                    loyaltyResult.DiscountPercentage = LoyaltyDiscountPercent;
                    loyaltyResult.DiscountAmount = loyaltyDiscount.Value;
                    loyaltyResult.DiscountedTotal = subtotal - loyaltyDiscount.Value;
                    loyaltyResult.Message = $"Loyalty discount applied! You saved {(loyaltyDiscount.Value / 100.0m):C}";
                    
                    _logger.LogInformation("Loyalty discount of {Discount:C} applied for customer {CustomerName}", 
                        loyaltyDiscount.Value / 100.0m, customer.FullName);
                }
                else
                {
                    int shortfall = (LoyaltyThresholdCents + 1) - subtotal;
                    loyaltyResult.Message = $"Add {(shortfall / 100.0m):C} more to get {LoyaltyDiscountPercent}% loyalty discount!";
                }

                int finalTotal = loyaltyResult.DiscountedTotal;

                // FIXED: Simple payment status - Paid or Unpaid only (no AmountPaid)
                SalesInvoicePaymentStatus paymentStatus;
                if (dto.IsCredit)
                    paymentStatus = SalesInvoicePaymentStatus.Unpaid;
                else
                    paymentStatus = SalesInvoicePaymentStatus.Paid;

                var invoice = new SalesInvoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    CustomerId = dto.CustomerId,
                    Customer = customer,
                    CreatedByStaffId = dto.CreatedByStaffId,
                    CreatedByStaff = staff,
                    PaymentStatus = paymentStatus,
                    Subtotal = subtotal,
                    LoyaltyDiscount = loyaltyDiscount,
                    FinalTotal = finalTotal,
                    PaymentDueDate = dto.IsCredit ? DateTime.UtcNow.AddDays(30) : null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Items = items
                };

                _context.SalesInvoices.Add(invoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await CheckLowStock();

                _logger.LogInformation("Sales invoice {InvoiceNumber} created for customer {CustomerName}. Total: {Total:C}, Discount: {Discount:C}", 
                    invoiceNumber, customer.FullName, finalTotal / 100.0m, (loyaltyDiscount ?? 0) / 100.0m);

                return new SalesInvoiceResponseDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoiceDate = invoice.InvoiceDate,
                    CustomerName = customer.FullName,
                    CustomerEmail = customer.Email ?? "N/A",
                    StaffName = staff.FullName,
                    Subtotal = subtotal,
                    LoyaltyDiscount = loyaltyDiscount,
                    FinalTotal = finalTotal,
                    PaymentStatus = paymentStatus.ToString(),
                    PaymentDueDate = invoice.PaymentDueDate,
                    LoyaltyDiscountApplied = loyaltyResult.IsApplicable,
                    Items = items.Select(i => new SalesInvoiceItemResponseDto
                    {
                        PartName = i.Part.PartName,
                        PartNumber = i.Part.PartCode,
                        Quantity = i.Quantity,
                        UnitSellingPrice = i.UnitSellingPrice,
                        TotalPrice = i.Quantity * i.UnitSellingPrice
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating sales invoice");
                throw;
            }
        }

        private string GeneratePurchaseInvoiceNumber()
        {
            string yearMonth = DateTime.UtcNow.ToString("yyyyMM");
            var last = _context.PurchaseInvoices
                .Where(i => i.InvoiceNumber.StartsWith($"PO-{yearMonth}"))
                .OrderByDescending(i => i.Id)
                .FirstOrDefault();

            int seq = 1;
            if (last != null && last.InvoiceNumber.Length > 11)
            {
                string lastSeq = last.InvoiceNumber.Substring(11);
                if (int.TryParse(lastSeq, out int num)) 
                    seq = num + 1;
            }
            return $"PO-{yearMonth}-{seq:D4}";
        }

        private string GenerateSalesInvoiceNumber()
        {
            string yearMonth = DateTime.UtcNow.ToString("yyyyMM");
            var last = _context.SalesInvoices
                .Where(i => i.InvoiceNumber.StartsWith($"INV-{yearMonth}"))
                .OrderByDescending(i => i.Id)
                .FirstOrDefault();

            int seq = 1;
            if (last != null && last.InvoiceNumber.Length > 12)
            {
                string lastSeq = last.InvoiceNumber.Substring(12);
                if (int.TryParse(lastSeq, out int num)) 
                    seq = num + 1;
            }
            return $"INV-{yearMonth}-{seq:D4}";
        }

        private async Task CheckLowStock()
        {
            var lowStockParts = await _context.Parts
                .Where(p => p.StockQuantity < 10)
                .ToListAsync();

            foreach (var part in lowStockParts)
            {
                _logger.LogWarning("LOW STOCK ALERT: Part '{PartName}' (ID: {Id}) has only {Stock} units remaining.", 
                    part.PartName, part.Id, part.StockQuantity);
            }
        }
    }
}
