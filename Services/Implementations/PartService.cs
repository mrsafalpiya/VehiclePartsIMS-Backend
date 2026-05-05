using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class PartService : IPartService
    {
        private readonly AppDbContext _context;

        public PartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PartResponseDto>> GetAllAsync(int? vendorId = null)
        {
            var query = _context.Parts.Include(p => p.Vendor).AsQueryable();
            if (vendorId.HasValue)
                query = query.Where(p => p.VendorId == vendorId.Value);
            return await query.Select(p => MapToDto(p)).ToListAsync();
        }

        public async Task<PartResponseDto?> GetByIdAsync(int id)
        {
            var part = await _context.Parts
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Id == id);

            return part is null ? null : MapToDto(part);
        }

        public async Task<(bool Success, string Message, PartResponseDto? Data)> CreateAsync(CreatePartDto dto)
        {
            // Business rule: part code must be unique
            bool codeExists = await _context.Parts.AnyAsync(p => p.PartCode == dto.PartCode);
            if (codeExists)
                return (false, "Part code already exists.", null);

            // Validate vendor exists
            bool vendorExists = await _context.Vendors.AnyAsync(v => v.Id == dto.VendorId);
            if (!vendorExists)
                return (false, "Vendor not found.", null);

            // Validate selling price and stock
            if (dto.SellingPrice <= 0)
                return (false, "Selling price must be greater than 0.", null);
            if (dto.StockQuantity < 0)
                return (false, "Stock quantity cannot be negative.", null);

            var part = new Part
            {
                PartName = dto.PartName,
                PartCode = dto.PartCode,
                SellingPrice = dto.SellingPrice,
                StockQuantity = dto.StockQuantity,
                VendorId = dto.VendorId
            };

            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            // Reload with vendor name for response
            await _context.Entry(part).Reference(p => p.Vendor).LoadAsync();
            return (true, "Part created successfully.", MapToDto(part));
        }

        public async Task<(bool Success, string Message, PartResponseDto? Data)> UpdateAsync(int id, UpdatePartDto dto)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part is null)
                return (false, "Part not found.", null);

            // Business rule: part code must be unique (excluding itself)
            bool codeExists = await _context.Parts
                .AnyAsync(p => p.PartCode == dto.PartCode && p.Id != id);
            if (codeExists)
                return (false, "Part code already exists.", null);

            bool vendorExists = await _context.Vendors.AnyAsync(v => v.Id == dto.VendorId);
            if (!vendorExists)
                return (false, "Vendor not found.", null);

            if (dto.SellingPrice <= 0)
                return (false, "Selling price must be greater than 0.", null);
            if (dto.StockQuantity < 0)
                return (false, "Stock quantity cannot be negative.", null);

            part.PartName = dto.PartName;
            part.PartCode = dto.PartCode;
            part.SellingPrice = dto.SellingPrice;
            part.StockQuantity = dto.StockQuantity;
            part.VendorId = dto.VendorId;

            await _context.SaveChangesAsync();
            await _context.Entry(part).Reference(p => p.Vendor).LoadAsync();
            return (true, "Part updated successfully.", MapToDto(part));
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part is null)
                return (false, "Part not found.");

            // Business rule: cannot delete if part appears in any invoice
            // Uncomment when invoice entities are added in later milestone:
            // bool usedInInvoice = await _context.SalesInvoiceItems.AnyAsync(i => i.PartId == id)
            //                   || await _context.PurchaseInvoiceItems.AnyAsync(i => i.PartId == id);
            // if (usedInInvoice)
            //     return (false, "Cannot delete part that appears in an existing invoice.");

            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
            return (true, "Part deleted successfully.");
        }

        private static PartResponseDto MapToDto(Part p) => new()
        {
            Id = p.Id,
            PartName = p.PartName,
            PartCode = p.PartCode,
            SellingPrice = p.SellingPrice,
            StockQuantity = p.StockQuantity,
            VendorId = p.VendorId,
            VendorName = p.Vendor?.VendorName ?? string.Empty
        };
    }
}
