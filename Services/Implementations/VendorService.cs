using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class VendorService: IVendorService
    {
        private readonly AppDbContext _context;

        public VendorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VendorResponseDto>> GetAllAsync(string? search)
        {
            var query = _context.Vendors.AsQueryable();

            // Filter by name if search term is provided
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(v => v.VendorName.ToLower().Contains(search.ToLower()));

            return await query
                .Select(v => MapToDto(v))
                .ToListAsync();
        }

        public async Task<VendorResponseDto?> GetByIdAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            return vendor is null ? null : MapToDto(vendor);
        }

        public async Task<VendorResponseDto> CreateAsync(CreateVendorDto dto)
        {
            var vendor = new Vendor
            {
                VendorName = dto.VendorName,
                ContactPersonName = dto.ContactPersonName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            return MapToDto(vendor);
        }

        public async Task<VendorResponseDto?> UpdateAsync(int id, UpdateVendorDto dto)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor is null) return null;

            vendor.VendorName = dto.VendorName;
            vendor.ContactPersonName = dto.ContactPersonName;
            vendor.Email = dto.Email;
            vendor.PhoneNumber = dto.PhoneNumber;
            vendor.Address = dto.Address;

            await _context.SaveChangesAsync();
            return MapToDto(vendor);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor is null)
                return (false, "Vendor not found.");

            // Business rule: cannot delete if linked to a purchase invoice
            // (Purchase invoices are added in a later milestone — placeholder check below)
            // bool hasInvoices = await _context.PurchaseInvoices.AnyAsync(p => p.VendorId == id);
            // if (hasInvoices) return (false, "Cannot delete vendor linked to a purchase invoice.");

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return (true, "Vendor deleted successfully.");
        }

        private static VendorResponseDto MapToDto(Vendor v) => new()
        {
            Id = v.Id,
            VendorName = v.VendorName,
            ContactPersonName = v.ContactPersonName,
            Email = v.Email,
            PhoneNumber = v.PhoneNumber,
            Address = v.Address
        };
    }
}
